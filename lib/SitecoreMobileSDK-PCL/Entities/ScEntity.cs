
namespace Sitecore.MobileSDK.Entities
{
  using System;
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Fields;

  //TODO: @igk id field is required for entities implement it as property

  public class ScEntity : ISitecoreEntity
  {

    private const string EntityIDKey = "Id";

    public ScEntity()
    {
    }

    public string Id {
      get {
        return this.FieldWithName(EntityIDKey).RawValue;
      }
    }

    #region Class variables;

    public IEnumerable<IField> Fields { get; private set; }

    public IField this[string caseInsensitiveFieldName] {
      get {
        return this.FieldWithName(caseInsensitiveFieldName);
      }
    }

    public int FieldsCount {
      get {
        return this.FieldsByName.Count;
      }
    }

    private Dictionary<string, IField> FieldsByName { get; set; }

    public IField FieldWithName(string caseInsensitiveFieldName)
    {
      string lowercaseName = caseInsensitiveFieldName.ToLowerInvariant();
      return this.FieldsByName[lowercaseName];
    }

    #endregion Class variables;

    public ScEntity(Dictionary<string, IField> fieldsByName)
    {
      this.FieldsByName = fieldsByName;

      int fieldsCount = fieldsByName.Count;
      IField[] fields = new IField[fieldsCount];
      fieldsByName.Values.CopyTo(fields, 0);
      this.Fields = fields;
    }
  }
}
