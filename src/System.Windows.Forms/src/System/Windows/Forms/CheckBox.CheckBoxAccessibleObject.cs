﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using static Interop;

namespace System.Windows.Forms
{
    public partial class CheckBox
    {
        public class CheckBoxAccessibleObject : ButtonBaseAccessibleObject
        {
            private readonly CheckBox _owningCheckBox;

            public CheckBoxAccessibleObject(Control owner)
                : base((owner is CheckBox owningCheckBox) ? owner : throw new ArgumentException(string.Format(SR.ConstructorArgumentInvalidValueType, nameof(Owner), typeof(CheckBox))))
            {
                _owningCheckBox = owningCheckBox;
            }

            public override string DefaultAction
            {
                get
                {
                    string? defaultAction = Owner.AccessibleDefaultActionDescription;

                    if (defaultAction is not null)
                    {
                        return defaultAction;
                    }

                    return _owningCheckBox.Checked
                        ? SR.AccessibleActionUncheck
                        : SR.AccessibleActionCheck;
                }
            }

            public override AccessibleRole Role
            {
                get
                {
                    AccessibleRole role = Owner.AccessibleRole;
                    return role != AccessibleRole.Default
                        ? role
                        : AccessibleRole.CheckButton;
                }
            }

            public override AccessibleStates State
                => _owningCheckBox.CheckState switch
                {
                    CheckState.Checked => AccessibleStates.Checked | base.State,
                    CheckState.Indeterminate => AccessibleStates.Indeterminate | base.State,
                    _ => base.State
                };

            internal override UiaCore.ToggleState ToggleState
                => _owningCheckBox.Checked
                    ? UiaCore.ToggleState.On
                    : UiaCore.ToggleState.Off;

            internal override bool IsPatternSupported(UiaCore.UIA patternId)
                => patternId switch
                {
                    var p when
                        p == UiaCore.UIA.TogglePatternId => true,
                    _ => base.IsPatternSupported(patternId)
                };

            internal override object? GetPropertyValue(UiaCore.UIA propertyID)
                => propertyID switch
                {
                    UiaCore.UIA.HasKeyboardFocusPropertyId => Owner.Focused,
                    UiaCore.UIA.IsKeyboardFocusablePropertyId
                        =>
                        // This is necessary for compatibility with MSAA proxy:
                        // IsKeyboardFocusable = true regardless the control is enabled/disabled.
                        true,
                    _ => base.GetPropertyValue(propertyID)
                };

            public override void DoDefaultAction()
            {
                if (_owningCheckBox.IsHandleCreated)
                {
                    _owningCheckBox.AccObjDoDefaultAction = true;
                }

                try
                {
                    base.DoDefaultAction();
                }
                finally
                {
                    if (_owningCheckBox.IsHandleCreated)
                    {
                        _owningCheckBox.AccObjDoDefaultAction = false;
                    }
                }
            }

            internal override void Toggle()
            {
                _owningCheckBox.Checked = !_owningCheckBox.Checked;
            }
        }
    }
}
