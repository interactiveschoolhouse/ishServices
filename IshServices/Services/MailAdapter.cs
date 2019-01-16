using System.Threading.Tasks;

namespace IshServices.Services
{
    public interface MailAdapter
    {
        void Send(SiteMessage siteMessage);
    }
}