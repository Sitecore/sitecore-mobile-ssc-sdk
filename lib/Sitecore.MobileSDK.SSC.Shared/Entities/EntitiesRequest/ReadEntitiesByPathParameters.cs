
namespace Sitecore.MobileSDK.Entities
{
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.API.Request;
  using Sitecore.MobileSDK.API.Request.Entity;
  using Sitecore.MobileSDK.API.Request.Parameters;

  public class ReadEntitiesByPathParameters : IReadEntitiesByPathRequest
  {
    public ReadEntitiesByPathParameters(IEntitySource entitySource, ISessionConfig sessionConfig, IDictionary<string, string> parametersRawValuesByName)
    {
      this.EntitySource = entitySource;
      this.SessionSettings = sessionConfig;
      this.ParametersRawValuesByName = parametersRawValuesByName;
    }

    public virtual IReadEntitiesByPathRequest DeepCopyReadEntitiesByPathRequest()
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

      return new ReadEntitiesByPathParameters(entitySource, sessionSettings, this.ParametersRawValuesByName);
    }

    public IEntitySource EntitySource { get; private set; }
    public IDictionary<string, string> ParametersRawValuesByName { get; private set; }

    //FIXME: @igk exclude IBaseItemRequest from parents and properties below

    public virtual IBaseItemRequest DeepCopyBaseGetItemRequest()
    {
      return this.DeepCopyReadEntitiesByPathRequest();
    }

    public string ItemPath { get; private set; }
    public IItemSource ItemSource { get; private set; }
    public ISessionConfig SessionSettings { get; private set; }
    public IQueryParameters QueryParameters { get; private set; }
    public bool IncludeStandardTemplateFields { get; private set; }
  }
}