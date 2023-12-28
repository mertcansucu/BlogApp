using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface ICommentRepository
    {//IPostRepository =>burada bazı kullanacağım metotları yazıcam ama bunlar sanal olacak, yani interfacede kullanacağım metot tanımlamaları bunları da concrete içinde gerçek bir metoda dönüştürücem
        IQueryable<Comment> Comments { get; }

        void CreateComment(Comment comment);
    }
}