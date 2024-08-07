//Copyright 2023 Daniil Glagolev
//Licensed under the Apache License, Version 2.0

using Core.Scripts.Localizations.Unity.Base;
using UnityEngine;

namespace LocalizationsTools_V2.Unity.Text
{
    [RequireComponent(typeof(LocalizationInfo))]
    public sealed class TMPTextLocalization : AbstractTextLocalization
    {
        protected override string GetText(string original) => original;
    }
}
