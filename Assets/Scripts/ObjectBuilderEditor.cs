using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Game))]
public class ObjectBuilderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Game myScript = (Game)target;
		if(GUILayout.Button("Generate Squares"))
		{
			myScript.GenerateGrid();
		}
	}
}