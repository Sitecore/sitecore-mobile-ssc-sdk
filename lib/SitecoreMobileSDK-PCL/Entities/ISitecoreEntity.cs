namespace Sitecore.MobileSDK.API.Entities
{
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Fields;

  public interface ISitecoreEntity
  {
    string Id { get; }

    int FieldsCount { get; }

    IField this[string caseInsensitiveFieldName] { get; }

    IEnumerable<IField> Fields{ get; }
  }
}

