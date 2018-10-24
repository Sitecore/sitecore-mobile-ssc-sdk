
namespace Sitecore.MobileSDK.Entities
{
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.API.Request.Parameters;

  public class ChangeEntitiesParameters : IUpdateEntityRequest
  {
    public ChangeEntitiesParameters(string id, IDictionary<string, string> fieldsRawValuesByName, IDictionary<string, string> parametersRawValuesByName, IEntitySource entitySource)
    {
      this.EntitySource = entitySource;
      this.EntityID = id;
      this.FieldsRawValuesByName = fieldsRawValuesByName;
      this.ParametersRawValuesByName = parametersRawValuesByName;
    }

    public ChangeEntitiesParameters(string id, IDictionary<string, string> fieldsRawValuesByName, IDictionary<string, string> parametersRawValuesByName, IEntitySource entitySource, ISessionConfig sessionSettings)
    {
      this.EntitySource = entitySource;
      this.EntityID = id;
      this.FieldsRawValuesByName = fieldsRawValuesByName;
      this.ParametersRawValuesByName = parametersRawValuesByName;
      this.SessionSettings = sessionSettings;
    }

    public IUpdateEntityRequest DeepCopyUpdateEntityRequest()
    { 
      IEntitySource entitySource = null;

      if (null != this.EntitySource) {
        entitySource = this.EntitySource.ShallowCopy();
      }

      return new ChangeEntitiesParameters(this.EntityID, this.FieldsRawValuesByName, this.ParametersRawValuesByName, entitySource);
    }

    public ICreateEntityRequest DeepCopyCreateEntityRequest()
    { 
      return this.DeepCopyUpdateEntityRequest();
    }

    public virtual IReadEntityByIdRequest DeepCopyReadEntitiesByIdRequest()
    {
      return this.DeepCopyUpdateEntityRequest();
    }

    public string EntityID { get; protected set; }
    public IDictionary<string, string> FieldsRawValuesByName { get; protected set; }
    public IDictionary<string, string> ParametersRawValuesByName { get; private set; }

    public IEntitySource EntitySource { get; protected set; }

    //FIXME: @igk exclude IBaseItemRequest from parents and properties below

    public virtual IBaseItemRequest DeepCopyBaseGetItemRequest()
    {
      return this.DeepCopyUpdateEntityRequest();
    }

    public string ItemPath { get; protected set; }
    public IItemSource ItemSource { get; protected set; }
    public ISessionConfig SessionSettings { get; protected set; }
    public IQueryParameters QueryParameters { get; protected set; }
    public bool IncludeStandardTemplateFields { get; protected set; }
  }
}
