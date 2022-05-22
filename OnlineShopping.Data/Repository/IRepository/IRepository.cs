using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping.Data.Repository.IRepository
{
    //T: Buraya gelecek olan modeli(category,product,order ...) temsil ediyor.
    public interface IRepository<T> where T : class
    {
        //CREATE İŞLEMLERİ İÇİN
        //veritabanına ekleme yapmak için 
        void Add(T entity);


        //READ İŞLEMLERİ İÇİN
        //Tek bir sorgu almamıza yarıyor. Yani productId ye göre o productı getir gibi. 
        T GetFirstOrDefault(Expression<Func<T,bool>> filter, string includeProperties = null);

        //Birden fazla kayıt listemek için bunu kullanıyoruz.
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string includeProperties = null); 
        
        //UPDATE İŞLEMLERİ İÇİN
        void Update(T entity);  

        //DELETE İŞLEMLERİ İÇİN
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities); //Burada da kategorilerin hepsini silmek istersek bu yapıyı kullancaz


    }
}
