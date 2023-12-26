using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface ITagRepository
    {//ITagRepository =>burada bazı kullanacağım metotları yazıcam ama bunlar sanal olacak, yani interfacede kullanacağım metot tanımlamaları bunları da concrete içinde gerçek bir metoda dönüştürücem
        IQueryable<Tag> Tags { get; }//IQueryable=>Contex üzerinden Post verilerini aldığımda extra filtreleme yapabilmek için

        void CreateTag(Tag tag);
    }
}