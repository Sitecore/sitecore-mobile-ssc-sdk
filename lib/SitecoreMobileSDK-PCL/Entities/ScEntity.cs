
namespace Sitecore.MobileSDK.Entities
{
  using System.Collections.Generic;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Fields;

  //TODO: @igk add possibility to change id field key value

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

    public IField this[string caseSensitiveFieldName] {
      get {
        return this.FieldWithName(caseSensitiveFieldName);
      }
    }

    public int FieldsCount {
      get {
        return this.FieldsByName.Count;
      }
    }

    private Dictionary<string, IField> FieldsByName { get; set; }

    public IField FieldWithName(string caseSensitiveFieldName)
    {
      return this.FieldsByName[caseSensitiveFieldName];
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
