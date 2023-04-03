using AwesomeNetwork.DAL.Data.Repository;
using AwesomeNetwork.DAL.Models.Users;

namespace AwesomeNetwork.Data.Repository
{
    public class MessageRepository : Repository<Message>
    {
        public MessageRepository(DAL.ApplicationDbContext db) : base(db)
        {
        }
    }
}
