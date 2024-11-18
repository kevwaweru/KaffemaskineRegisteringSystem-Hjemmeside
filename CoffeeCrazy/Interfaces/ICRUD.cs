namespace CoffeeCrazy.Interfaces
{
    //Kevin
    public interface ICRUD<T>
    {
        //Create
        void Create(T toBeCreatedT);

        //Read
        List<T> GetAll();
        T GetById(int id);

        //Update
        void Update(T toBeUpdatedT);

        //Delete
        void Delete(T toBeDeletedT);




    }
}
