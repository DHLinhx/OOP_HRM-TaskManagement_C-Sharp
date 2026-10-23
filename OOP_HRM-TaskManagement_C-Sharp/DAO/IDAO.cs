namespace com.hrmis.dao;

public interface IDAO<T, ID>
{
    List<T> GetAll();
    T? GetById(ID id);
    void Add(T entity);
    void Update(T entity);
    bool Delete(ID id);
}
