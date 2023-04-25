using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tessera
{
    /// <summary>
    /// The sole purpose of this class is to warn users upgrading from Tessera to Tessera Pro that references have changed.
    /// After upgrading, you can delete anything labeled Dummy.
    /// </summary>
    [CustomEditor(typeof(Dummy_3092287824))]
    public class DummyTesseraGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("If you are reading this, you need to follow the \"Upgrading to Tessera Pro\" instructions in the documentation.", MessageType.Error);
        }
    }
}