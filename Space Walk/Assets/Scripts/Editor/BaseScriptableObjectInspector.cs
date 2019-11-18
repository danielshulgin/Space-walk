using System;
using ItemSystem;
using ItemSystem.ScriptableObjects.ConcreteScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor( typeof( BaseScriptableObject ) )]
    public class BaseScriptableObjectEditor : UnityEditor.Editor
    {
        public void OnEnable()
        {
            var baseScriptableObject = (BaseScriptableObject)target;
            if ( baseScriptableObject.Guid == System.Guid.Empty )
            {
                baseScriptableObject.GenerateGuid();
                EditorUtility.SetDirty( target );
            }
            EditorUtility.SetDirty(target);
        }
    }

    [CustomEditor( typeof( GunScriptableObject) )]
    public class PistolScriptableObjectEditor : BaseScriptableObjectEditor
    {
        private new void OnEnable()
        {
            base.OnEnable();
        }
    }

    [CustomEditor( typeof( BulletScriptableObject) )]
    public class BulletScriptableObjectEditor : BaseScriptableObjectEditor
    {
        private new void OnEnable()
        {
            base.OnEnable();
        }
    }
}