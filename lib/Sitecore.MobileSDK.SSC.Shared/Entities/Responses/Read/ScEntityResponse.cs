namespace Sitecore.MobileSDK.API.Entities
{
  using System.Collections.Generic;

  public class ScEntityResponse : IEnumerable<ISitecoreEntity>
  {
    
    public ScEntityResponse(List<ISitecoreEntity> entities, int statusCode)
    {
      this.StatusCode = statusCode;
      this.Entities = entities;
    }

    #region Paging

    public int ResultCount { 
      get {
        return this.Entities.Count;
      }
    }
    #endregion Paging

    #region IEnumerable 
    /// <summary>
    ///     Returns an enumerator that iterates through the items list. 
    ///     Note : Required by the compiler to conform the non-generic IEnumerable interface.
    /// </summary>
    /// <returns>
    ///      <see cref="IEnumerator{T}"/> that can be used to iterate through the items.
    /// </returns>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.Entities.GetEnumerator();
    }

    /// <summary>
    ///     Returns an enumerator that iterates through the items list.
    ///     A generic version of the iterator.    
    /// 
    /// </summary>
    /// <returns>
    ///      <see cref="IEnumerator{T}"/> that can be used to iterate through the items.
    /// </returns>
    public IEnumerator<ISitecoreEntity> GetEnumerator()
    {
      return this.Entities.GetEnumerator();
    }

    /// <summary>
    ///     Gets the item that was received.
    /// </summary>
    /// <param name="index">The index of item.</param>
    ///
    /// <returns>
    ///     <see cref="ISitecoreEntity"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"> index is less than 0 or index is equal to or greater than <see cref="List{T}.Count"/>.</exception>
    public ISitecoreEntity this[int index]
    {
      get
      {
        return this.Entities[index];
      }
    }

    private List<ISitecoreEntity> Entities
    {
      get;
      /*private*/
      set;
    }

    public int StatusCode {
      get {
        return this.statusCode;
      }

      private set {
        this.statusCode = value;
      }
    }

    private int statusCode = 0;
    #endregion IEnumerable
  }
}