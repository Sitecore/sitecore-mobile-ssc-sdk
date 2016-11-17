namespace Sitecore.MobileSdkUnitTest
{
  using NUnit.Framework;
  using Sitecore.MobileSDK.API.Entities;
  using Sitecore.MobileSDK.API.Items;
  using Sitecore.MobileSDK.Items;
  using Sitecore.MobileSDK.MockObjects;
  using Sitecore.MobileSDK.UserRequest;

  [TestFixture]
  public class EntitySourceMergerTest
  {
    [Test]
    public void TestEntitySourceMergerCopiesInputStruct()
    {
      IEntitySource source = new EntitySource("ns", "cnt", "id", "act" );
      var merger = new EntitySourceMerger(source);

      Assert.AreNotSame(source, merger.DefaultSource);
      Assert.AreEqual(source, merger.DefaultSource);
    }

    [Test]
    public void TestItemSourceMergerDefaultValuesAreOptional()
    {
      var result = new EntitySourceMerger(null);
      Assert.IsNotNull(result);
    }

    [Test]
    public void TestIdAreOptionalForDefaultSource()
    {
      IEntitySource source = new EntitySource("ns", "cnt", null, "act");

      Assert.DoesNotThrow( () => new EntitySourceMerger(source) );
    }

    [Test]
    public void TestMergerReturnsDefaultSourceCopyForNilInput()
    {
      IEntitySource source = new EntitySource("ns", "cnt", "id", "act");
      var merger = new EntitySourceMerger(source);

      IEntitySource result = merger.FillEntitySourceGaps(null);

      Assert.AreNotSame(source, result);
      Assert.AreEqual(source, result);
    }


   

    [Test]
    public void TestMergerReturnsNullIfBothInputAndDefaultAreNull()
    {
      var merger = new EntitySourceMerger(null);

      IEntitySource result = merger.FillEntitySourceGaps(null);

      Assert.IsNull(result);
    }

    [Test]
    public void TestUserFieldsHaveHigherPriority()
    {
      IEntitySource source = new EntitySource("ns", "cnt", "id", "act");
      IEntitySource userSource = new EntitySource("userns", "usercnt", "userid", "useract");


      var merger = new EntitySourceMerger(source);
      IEntitySource result = merger.FillEntitySourceGaps(userSource);

      Assert.AreEqual (userSource, result);
      Assert.AreNotSame (userSource, result);
    }

    [Test]
    public void TestNullUserFieldsAreAutocompleted()
    {
      IEntitySource source = new EntitySource("ns", "cnt", "id", "act");
      IEntitySource userSource = new EntitySource(null, null, null, null);


      var merger = new EntitySourceMerger(source);
      IEntitySource result = merger.FillEntitySourceGaps(userSource);

      Assert.AreEqual(source, result);
      Assert.AreNotSame(source, result);
    }
  }
}

