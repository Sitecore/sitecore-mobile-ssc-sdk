namespace Sitecore.MobileSDK.API.Entities
{
  using System;
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Fields;

  //TODO: can be the ISitecoreItem's parent!!!???
  public interface ISitecoreEntity
  {
    int FieldsCount { get; }

    IField this[string caseInsensitiveFieldName] { get; }

    IEnumerable<IField> Fields{ get; }
  }
}

