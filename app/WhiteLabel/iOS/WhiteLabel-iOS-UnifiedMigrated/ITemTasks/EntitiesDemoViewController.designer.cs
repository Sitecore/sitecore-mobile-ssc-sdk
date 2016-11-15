// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace WhiteLabeliOS
{
	[Register ("EntitiesDemoViewController")]
	partial class EntitiesDemoViewController
	{
		[Outlet]
		UIKit.UITextField EntityIdTextField { get; set; }

		[Outlet]
		UIKit.UITextField EntityTitleTextField { get; set; }

		[Action ("CreateButtonTouched:")]
		partial void CreateButtonTouched (Foundation.NSObject sender);

		[Action ("DeleteButtonTouched:")]
		partial void DeleteButtonTouched (Foundation.NSObject sender);

		[Action ("ReadAllButtonTouched:")]
		partial void ReadAllButtonTouched (Foundation.NSObject sender);

		[Action ("ReadByIdButtonTouched:")]
		partial void ReadByIdButtonTouched (Foundation.NSObject sender);

		[Action ("UpdateButtonTouched:")]
		partial void UpdateButtonTouched (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (EntityTitleTextField != null) {
				EntityTitleTextField.Dispose ();
				EntityTitleTextField = null;
			}

			if (EntityIdTextField != null) {
				EntityIdTextField.Dispose ();
				EntityIdTextField = null;
			}
		}
	}
}
