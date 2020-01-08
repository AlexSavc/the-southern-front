using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkinManager))]
public class SkinManagerEditor : Editor
{
    bool AmEditing = false;
    Rect rect;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SkinManager skinManager = (SkinManager)target;

        try
        {
            EditorGUILayout.LabelField("Current Skin: ", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(skinManager.currentSkin.skinName, EditorStyles.inspectorDefaultMargins);
        }
        catch (System.Exception) { }
        

        if (AmEditing)
        {
            try
            {
                SkinData edit = skinManager.editing;

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Editing Skin:", EditorStyles.boldLabel);
                EditorGUILayout.LabelField(edit.skinName, EditorStyles.wordWrappedLabel);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Skin Name: ");
                edit.skinName = EditorGUILayout.TextField(edit.skinName);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical("Box");
                EditorGUILayout.PrefixLabel("Panel Sprite");
                skinManager.panelSprite = (Sprite)EditorGUILayout.ObjectField(skinManager.panelSprite, typeof(Sprite), false);
                if(skinManager.panelSprite != null)
                {
                    rect = EditorGUILayout.GetControlRect();
                    rect = new Rect(rect.x, rect.y - 1, 100, 100);
                    EditorGUI.DrawPreviewTexture(rect, skinManager.panelSprite.texture,null , ScaleMode.ScaleToFit);
                    GUILayout.Space(82);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical("Box");
                EditorGUILayout.PrefixLabel("Button Sprite");
                skinManager.buttonSprite = (Sprite)EditorGUILayout.ObjectField(skinManager.buttonSprite, typeof(Sprite), false);
                if (skinManager.buttonSprite != null)
                {
                    rect = EditorGUILayout.GetControlRect();
                    EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y - 1, 100, 100), skinManager.buttonSprite.texture, null, ScaleMode.ScaleToFit);
                    GUILayout.Space(82);
                }
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical("Box");
                EditorGUILayout.PrefixLabel("Header Sprite");
                skinManager.headerSprite = (Sprite)EditorGUILayout.ObjectField(skinManager.headerSprite, typeof(Sprite), false);
                if (skinManager.headerSprite != null)
                {
                    rect = EditorGUILayout.GetControlRect();
                    EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y - 1, 100, 100), skinManager.headerSprite.texture, null, ScaleMode.ScaleToFit);
                    GUILayout.Space(82);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical("Box");
                EditorGUILayout.PrefixLabel("Tech. Button Sprite");
                skinManager.technicalButtonSprite = (Sprite)EditorGUILayout.ObjectField(skinManager.technicalButtonSprite, typeof(Sprite), false);
                if (skinManager.technicalButtonSprite != null)
                {
                    rect = EditorGUILayout.GetControlRect();
                    EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y - 1, 100, 100), skinManager.technicalButtonSprite.texture);
                    GUILayout.Space(82);
                }
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical("Box");
                EditorGUILayout.PrefixLabel("Element Sprite");
                skinManager.elementSprite = (Sprite)EditorGUILayout.ObjectField(skinManager.elementSprite, typeof(Sprite), false);
                if (skinManager.elementSprite != null)
                {
                    rect = EditorGUILayout.GetControlRect();
                    EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y - 1, 100, 100), skinManager.elementSprite.texture);
                    GUILayout.Space(82);
                }
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }
            catch (System.Exception) { }
            
            if (GUILayout.Button("Save Edits"))
            {
                AmEditing = false;
                skinManager.SaveSkinEdits();
                skinManager.GetSaves();
            }
        }

        SkinData skinData = null;

        EditorGUILayout.LabelField("Skins", EditorStyles.boldLabel);
        if(skinManager.skins != null && skinManager.skins.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();
            GUIContent skinList = new GUIContent("Skins");
            skinManager.skinIndex = EditorGUILayout.Popup(skinList, skinManager.skinIndex, skinManager.SkinsToArray());
            skinData = skinManager.skins[skinManager.skinIndex];
            if (GUILayout.Button("Delete"))
            {
                if (EditorUtility.DisplayDialog("Delete Skin?",
                                           "Are you sure you want to delete this Skin?",
                                           "Delete", "Cancel") == false) { return; }

                try
                {
                    skinManager.skins.Remove(skinData);
                }
                catch (System.ArgumentOutOfRangeException e) { Debug.Log(e); }
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("New Skin"))
        {
            skinManager.AddNewSkin();
        }
        if (GUILayout.Button("Edit Skin"))
        {
            AmEditing = true;
            skinManager.SetEditSkin(skinData);
        }
        if (GUILayout.Button("Set Active"))
        {
            skinManager.SetCurrentSkin(skinData);
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Text type IDs", EditorStyles.boldLabel);
        GUIContent idList = new GUIContent("Text type IDs");

        string textIDName = "no names";
            
        if (skinManager.textTypeIDs.Count > 0)
        {
            skinManager.textIndex = EditorGUILayout.Popup(idList, skinManager.textIndex, skinManager.textTypeIDs.ToArray());
            textIDName = skinManager.textTypeIDs[skinManager.textIndex];
        }
        else { EditorGUILayout.LabelField("No text types. Please create new", EditorStyles.wordWrappedLabel); }
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("New Text Type ID"))
        {
            skinManager.AddNewTextID();
        }

        if (GUILayout.Button("Remove this Text Type"))
        {
            if (EditorUtility.DisplayDialog("Remove Text Type?",
                                           "Are you sure you want to remove this Type?",
                                           "Remove", "Cancel") == false) { return; }

            try
            {
                skinManager.textTypeIDs.Remove(textIDName);
            }
            catch (System.ArgumentOutOfRangeException e) { Debug.Log(e); }
            //OnInspectorGUI();
        }
        GUILayout.EndHorizontal();

        SetEditingDisplay(skinManager.editing, skinManager);

        SetSavePathDisplay(skinManager);
    }

    public void SetEditingDisplay(SkinData skinData, SkinManager skinManager)
    {
        if (skinData == null) return;
        skinManager.SetSkinDataColorList(skinData);
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Text Color");
        skinData.colors[skinManager.textIndex] = EditorGUILayout.ColorField(skinData.colors[skinManager.textIndex]);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Edit ID name");
        skinManager.onTextIDNameEdit = EditorGUILayout.TextField(skinManager.onTextIDNameEdit);
        if (GUILayout.Button("Save"))
        {
            skinManager.ConfirmTextIDNameEdit();
        }
        //textIDName = EditorGUILayout.TextField(textIDName);
        GUILayout.EndHorizontal();
    }

    public void SetSavePathDisplay(SkinManager skinManager)
    {
        string savePath = skinManager.savePath;
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Save Path");
        EditorGUILayout.SelectableLabel(savePath);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Refresh Skin List"))
        {
            skinManager.GetSaves();
        }
    }
}
