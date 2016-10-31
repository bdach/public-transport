using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    public class UserService
    {
        public User Create(User user)
        {
            using (var db = new PublicTransportContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
                return user;
            }
        }

        public User Read(int id)
        {
            using (var db = new PublicTransportContext())
            {
                return db.Users.FirstOrDefault(u => u.Id == id);
            }
        }

        public User Update(User user)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(user.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).CurrentValues.SetValues(user);
                db.SaveChanges();
                return user;
            }
        }

        public void Delete(User user)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(user.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
