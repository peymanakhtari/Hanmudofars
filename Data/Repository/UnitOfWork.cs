using hanmudo.Data;
using hanmudo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RadicalTherapy.Data.Repository
{
    public class UnitOfWork : IDisposable
    {
        private AppDbContext context = new AppDbContext();
        private GenericRepository<ContentTeknik> _ContentTeknikRepository;
        private GenericRepository<Event> _EventRepository;
        private GenericRepository<Info> _InfoRepository;
        private GenericRepository<KeyValue> _KeyValueRepository;
        private GenericRepository<Photo> _PhotoRepository;
        private GenericRepository<Teknik> _TeknikRepository;
        private GenericRepository<User> _UserRepository;
        private GenericRepository<Video> _VideoRepository;
        private GenericRepository<Belt> _BeltRepository;
        private GenericRepository<Darian> _DarianRepository;
        private GenericRepository<CategoryTeknik> _CategoryTeknikRepository;
        private GenericRepository<SelfDefence> _SelfDefenceRepository;
        private GenericRepository<InfoUser> _InfoUserRepository;
        private GenericRepository<SeenInfoUser> _SeenInfoUserRepository;
        private GenericRepository<RoleContent> _RoleContentRepository;


        private bool disposed = false;
        public GenericRepository<KeyValue> KeyValueRepository
        {
            get
            {
                if (this._KeyValueRepository == null)
                {
                    this._KeyValueRepository = new GenericRepository<KeyValue>(context);
                }
                return _KeyValueRepository;
            }
        }
        public GenericRepository<RoleContent> RoleContentRepository
        {
            get
            {
                if (this._RoleContentRepository == null)
                {
                    this._RoleContentRepository = new GenericRepository<RoleContent>(context);
                }
                return _RoleContentRepository;
            }
        }
        public GenericRepository<SeenInfoUser> SeenInfoUserRepository
        {
            get
            {
                if (this._SeenInfoUserRepository == null)
                {
                    this._SeenInfoUserRepository = new GenericRepository<SeenInfoUser>(context);
                }
                return _SeenInfoUserRepository;
            }
        }
        public GenericRepository<InfoUser> InfoUserRepository
        {
            get
            {
                if (this._InfoUserRepository == null)
                {
                    this._InfoUserRepository = new GenericRepository<InfoUser>(context);
                }
                return _InfoUserRepository;
            }
        }
        public GenericRepository<SelfDefence> SelfDefenceTeknikRepository
        {
            get
            {
                if (this._SelfDefenceRepository == null)
                {
                    this._SelfDefenceRepository = new GenericRepository<SelfDefence>(context);
                }
                return _SelfDefenceRepository;
            }
        }
        public GenericRepository<CategoryTeknik> CategoryTeknikRepository
        {
            get
            {
                if (this._CategoryTeknikRepository == null)
                {
                    this._CategoryTeknikRepository = new GenericRepository<CategoryTeknik>(context);
                }
                return _CategoryTeknikRepository;
            }
        }
        public GenericRepository<Darian> DarianRepository
        {
            get
            {
                if (this._DarianRepository == null)
                {
                    this._DarianRepository = new GenericRepository<Darian>(context);
                }
                return _DarianRepository;
            }
        }
        public GenericRepository<ContentTeknik> ContentTeknikRepository
        {
            get
            {
                if (this._ContentTeknikRepository == null)
                {
                    this._ContentTeknikRepository = new GenericRepository<ContentTeknik>(context);
                }
                return _ContentTeknikRepository;
            }
        }
        public GenericRepository<Event> EventRepository
        {
            get
            {
                if (this._EventRepository == null)
                {
                    this._EventRepository = new GenericRepository<Event>(context);
                }
                return _EventRepository;
            }
        }
        public GenericRepository<Info> InfoRepository
        {
            get
            {
                if (this._InfoRepository == null)
                {
                    this._InfoRepository = new GenericRepository<Info>(context);
                }
                return _InfoRepository;
            }
        }
        public GenericRepository<KeyValue> Repository
        {
            get
            {
                if (this._KeyValueRepository == null)
                {
                    this._KeyValueRepository = new GenericRepository<KeyValue>(context);
                }
                return _KeyValueRepository;
            }
        }
        public GenericRepository<Photo> PhotoRepository
        {
            get
            {
                if (this._PhotoRepository == null)
                {
                    this._PhotoRepository = new GenericRepository<Photo>(context);
                }
                return _PhotoRepository;
            }
        }
        public GenericRepository<Teknik> TeknikRepository
        {
            get
            {
                if (this._TeknikRepository == null)
                {
                    this._TeknikRepository = new GenericRepository<Teknik>(context);
                }
                return _TeknikRepository;
            }
        }
        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this._UserRepository == null)
                {
                    this._UserRepository = new GenericRepository<User>(context);
                }
                return _UserRepository;
            }
        }
        public GenericRepository<Video> VideoRepository
        {
            get
            {
                if (this._VideoRepository == null)
                {
                    this._VideoRepository = new GenericRepository<Video>(context);
                }
                return _VideoRepository;
            }
        }
        public GenericRepository<Belt> BeltRepository
        {
            get
            {
                if (this._BeltRepository == null)
                {
                    this._BeltRepository = new GenericRepository<Belt>(context);
                }
                return _BeltRepository;
            }
        }
        public void Save()
        {
            context.SaveChanges();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
