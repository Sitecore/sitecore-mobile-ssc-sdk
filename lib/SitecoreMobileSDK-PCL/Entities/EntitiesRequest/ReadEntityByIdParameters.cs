
namespace Sitecore.MobileSDK.Entities
{
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.API.Request.Parameters;

  public class ReadEntityByIdParameters : IReadEntityByIdRequest
  {
    public ReadEntityByIdParameters(string id, IEntitySource entitySource, ISessionConfig sessionConfig)
    {
      this.EntityID = id;
      this.EntitySource = entitySource;
      this.SessionSettings = sessionConfig;
    }

    public ReadEntityByIdParameters(string id, IEntitySource entitySource)
    {
      this.EntityID = id;
      this.EntitySource = entitySource;
    }

    public virtual IReadEntityByIdRequest DeepCopyReadEntitiesByIdRequest()
    {
      IEntitySource entitySource = null;
      ISessionConfig sessionSettings = null;

      if (null != this.EntitySource)
      {
        entitySource = this.EntitySource.ShallowCopy();
      }

      if (null != this.SessionSettings) {
        sessionSettings = this.SessionSettings.SessionConfigShallowCopy();
      }

      return new ReadEntityByIdParameters(this.EntityID, entitySource, sessionSettings);
    }

    public IEntitySource EntitySource { get; private set; }

    public string EntityID { get; private set; }

    //FIXME: @igk exclude IBaseItemRequest from parents and properties below

    public virtual IBaseItemRequest DeepCopyBaseGetItemRequest()
    {
      return this.DeepCopyReadEntitiesByIdRequest();
    }

    public string ItemPath { get; private set; }
    public IItemSource ItemSource { get; private set; }
    public ISessionConfig SessionSettings { get; private set; }
    public IQueryParameters QueryParameters { get; private set; }
    public bool IncludeStandardTemplateFields { get; private set; }
  }
}