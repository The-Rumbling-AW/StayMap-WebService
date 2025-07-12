using backendAppsWeb.Communities.Domain.Model.Entity;
using backendAppsWeb.Communities.Domain.Model.ValueObjects;
using backendAppsWeb.Posts.Domain.Model.Aggregates;

namespace backendAppsWeb.Communities.Domain.Model.Aggregates;
public partial class Community
{
    public int Id { get; private set; }
    public string Name { get;  set; }
  //  public string MemberQuantity { get;  set; }
    public string Image { get;  set; }
    public string Description { get;  set; }
    
   // public Tags Tags { get;  set; }
    //public Members Members { get;  set; }
 
   public virtual ICollection<Post> Posts { get; private set; } = new List<Post>();
   
   public ICollection<CommunityMember> UserLinks { get; set; } = new List<CommunityMember>();


    public Community() { }

    public Community(string name,  string image, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la comunidad es obligatorio.", nameof(name));

        if (name.Length > 50)
            throw new ArgumentException("El nombre no puede exceder los 50 caracteres.", nameof(name));
        //  if (string.IsNullOrWhiteSpace(memberQuantity))
       //     throw new ArgumentException("La cantidad de miembros es obligatoria.", nameof(memberQuantity));

        if (string.IsNullOrWhiteSpace(image))
            throw new ArgumentException("La imagen es obligatoria.", nameof(image));

        if (string.IsNullOrWhiteSpace(description))
            description = "";

        Name = name;
       // MemberQuantity = memberQuantity;
        Image = image;
        Description = description;

      //  Tags = new Tags(tags);
        //Members = new Members(members);
        
    }


}
