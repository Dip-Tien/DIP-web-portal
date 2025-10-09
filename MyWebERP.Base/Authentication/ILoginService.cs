using MyWebERP.Model;

namespace MyWebERP.Authentication
{
    public interface ILoginService
    {
        Task<Model.LoginResultModel> Login(string username, string password, LanguageModel lang, int year);
        Task Logout();
    }
}
