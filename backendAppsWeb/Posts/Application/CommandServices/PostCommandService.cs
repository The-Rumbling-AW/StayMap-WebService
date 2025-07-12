using backendAppsWeb.Posts.Domain.Model.Aggregates;
using backendAppsWeb.Posts.Domain.Model.Commands;
using backendAppsWeb.Posts.Domain.Repositories;
using backendAppsWeb.Posts.Domain.Services.Command;
using backendAppsWeb.Communities.Domain.Repositories;
using backendAppsWeb.IAM.Domain.Repositories;
using backendAppsWeb.Shared.Domain.Repositories;

public class PostCommandService(
    IPostsRepository postRepository,
    ICommunityRepository communityRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IPostCommandService
{
    public async Task<Post?> Handle(CreatePostCommand command)
    {
        try
        {
            var community = await communityRepository.FindByIdAsync(command.CommunityId);
            if (community is null) return null;

            var user = await userRepository.FindByIdAsync(command.UserId);
            if (user is null) return null;

            var createdPost = new Post(community, command.UserId, command.Content, command.ImageUrl);
            await postRepository.AddAsync(createdPost);
            await unitOfWork.CompleteAsync(); // Post ya tiene ID

            // ✅ Agrega el ID del post al usuario
            user.PostsDone.Add(createdPost.Id);
            userRepository.Update(user); // 👈 sin await
            await unitOfWork.CompleteAsync(); // ✅ este sí se espera

            return createdPost;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR al crear post: " + ex.Message);
            return null;
        }
    }

    public async Task<bool> Handle(DeletePostCommand command)
    {
        var post = await postRepository.FindByIdAsync(command.Id);
        if (post is null) return false;

        // ✅ Buscar al usuario para eliminar el ID del post
        var user = await userRepository.FindByIdAsync(post.UserId);
        if (user != null)
        {
            user.PostsDone.PostIds.Remove(post.Id); // Eliminar el ID del post
            userRepository.Update(user);            // 👈 sin await
        }

        postRepository.Delete(post);
        await unitOfWork.CompleteAsync();

        return true;
    }
}
