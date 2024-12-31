using UnityEditor;
using FirstPersonMobileTools.DynamicFirstPerson;
using UnityEngine;

[CustomEditor(typeof(MovementController))]
public class MovementControllerEditor : Editor {

    public override void OnInspectorGUI() {
        
        serializedObject.Update();

        

        MovementController movementController = (MovementController)target;
        EditorGUILayout.Space(10f);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Animator"));


        EditorGUILayout.LabelField("Input Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Joystick"));

        EditorGUILayout.Space(10f);

        EditorGUILayout.LabelField("Ground Movement Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Acceleration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_WalkSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_RunSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_CrouchSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_CrouchDelay"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_CrouchHeight"));

        EditorGUILayout.Space(10f);

        EditorGUILayout.LabelField("Air Movement Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_JumpForce"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Gravity"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LandMomentum"));

        EditorGUILayout.Space(10f);

        EditorGUILayout.LabelField("Audio Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_FootStepSounds"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LandSound"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_JumpSound"));

        EditorGUILayout.Space(10f);

        EditorGUILayout.LabelField("Advanced Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_WalkBob"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_IdleBob"));

        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("Gun Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cameraTransform"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rayHittingRange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gunShotSound"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("muzzleFlash"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hitEffect"));


        serializedObject.ApplyModifiedProperties();

    }

}
