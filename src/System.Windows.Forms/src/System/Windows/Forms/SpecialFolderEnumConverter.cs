﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;

namespace System.Windows.Forms
{
    internal partial class SpecialFolderEnumConverter : EnumConverter
    {
        public SpecialFolderEnumConverter(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)]
            Type type) : base(type)
        {
        }

        /// <summary>
        ///  Personal appears twice in type editor because its numeric value matches with MyDocuments.
        ///  This code filters out the duplicate value.
        /// </summary>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            StandardValuesCollection values = base.GetStandardValues(context);
            var list = new ArrayList();
            int count = values.Count;
            bool personalSeen = false;
            for (int i = 0; i < count; i++)
            {
                if (values[i] is Environment.SpecialFolder specialFolder &&
                    specialFolder.Equals(Environment.SpecialFolder.Personal))
                {
                    if (!personalSeen)
                    {
                        personalSeen = true;
                        list.Add(values[i]);
                    }
                }
                else
                {
                    list.Add(values[i]);
                }
            }

            return new StandardValuesCollection(list);
        }

        protected override IComparer Comparer => SpecialFolderEnumComparer.Default;
    }
}
