using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// PlanetEditor class
[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;                  // reference to planet
    Editor shapeEditor;             // shape editor
    Editor colourEditor;            // colour editor

    // OnInspectorGUI method
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colourSettings, planet.OnColourSettingsUpdated, ref planet.colourSettingsFoldout, ref colourEditor);
    }

    // DrawSettingsEditor method
    // Editor for the shape and colour settings
    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            // fold out the settings arrow
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            // check if anything in the editor has changed
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();            // display editor

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    // OnEnable method
    private void OnEnable()
    {
        planet = (Planet)target;
    }
}