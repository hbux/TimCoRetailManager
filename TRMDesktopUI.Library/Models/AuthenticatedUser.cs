
namespace TRMDesktopUI.Library.Models
{
    public class AuthenticatedUser
    {
        // As the api returns multiple values we've created a model to store this information
        public string Access_Token { get; set; }
        public string UserName { get; set; }
    }
}
