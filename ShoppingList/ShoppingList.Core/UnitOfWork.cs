using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Minio;
using ShoppingList.Contracts;

namespace ShoppingList.Core
{
    public class UnitOfWork
    {
        public IUnitOfWork UnitofWork { get; set; }
        public IMapper Mapper { get; set; }
        public IMinioClient? MinioClient { get; set; }
    }
}
