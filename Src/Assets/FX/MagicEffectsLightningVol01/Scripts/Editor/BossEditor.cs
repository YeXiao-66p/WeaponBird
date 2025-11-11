using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Boss))]
public class BossEditor : Editor
{
    Boss Boss;

    Vector2 scrollPos;


    //private bool showBasicReferences = true;
    //private bool showPhaseSettings = true;
    //private bool showInvincibilitySettings = true;
    //private bool showDanmakuSettings = false;
    //private bool showMovementDanmaku = false;
    //private bool showCircleDanmaku = false;
    //private bool showGroupDanmaku = false;
    //private bool showRandomDanmaku = false;
    //private bool showSpiralDanmaku = false;
    //private bool showScatterDanmaku = false;
    //private bool showWaveDanmaku = false;
    //private bool showRotateDanmaku = false;
    //private bool showHighSpeedDanmaku = false;
    //private bool showDeathSettings = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Boss = (Boss)target;
        OnRulesGUI(Boss);

        //serializedObject.Update();

        //// 1. 基本引用
        //showBasicReferences = EditorGUILayout.Foldout(showBasicReferences, "🔗 基本引用", true);
        //if (showBasicReferences)
        //{
        //    EditorGUI.indentLevel++;
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("pool1Ani"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("danPre"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("danmuBluePre"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("danpreList"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("danmuRedPre"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("danmuPinkPreMove"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("danPreBlueMove"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletList2"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletTemplate2"));
        //    EditorGUI.indentLevel--;
        //    EditorGUILayout.Space();
        //}

        //// 2. Boss阶段设置
        //showPhaseSettings = EditorGUILayout.Foldout(showPhaseSettings, "📊 Boss阶段设置", true);
        //if (showPhaseSettings)
        //{
        //    EditorGUI.indentLevel++;
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("Phases"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("currentPhaseIndex"));
        //    EditorGUI.indentLevel--;
        //    EditorGUILayout.Space();
        //}

        //// 3. 无敌设置
        //showInvincibilitySettings = EditorGUILayout.Foldout(showInvincibilitySettings, "🛡️ 无敌设置", true);
        //if (showInvincibilitySettings)
        //{
        //    EditorGUI.indentLevel++;
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("isInvincible"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("invincibleTime"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("invincibleDuration"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("col"));
        //    EditorGUI.indentLevel--;
        //    EditorGUILayout.Space();
        //}

        //// 4. 弹幕设置总折叠
        //showDanmakuSettings = EditorGUILayout.Foldout(showDanmakuSettings, "🔫 弹幕系统设置", true);
        //if (showDanmakuSettings)
        //{
        //    EditorGUI.indentLevel++;

        //    // 4.1 移动弹幕球
        //    showMovementDanmaku = EditorGUILayout.Foldout(showMovementDanmaku, "🎯 移动弹幕球", true);
        //    if (showMovementDanmaku)
        //    {
        //        EditorGUI.indentLevel++;
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("fireCircleNewbulletsPerRing"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("totalRound"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("fireCircleNewringInterval"));
        //        EditorGUI.indentLevel--;
        //        EditorGUILayout.Space();
        //    }

        //    // 4.2 弹幕圈FireRotOldNWay
        //    showCircleDanmaku = EditorGUILayout.Foldout(showCircleDanmaku, "🌀 弹幕圈设置", true);
        //    if (showCircleDanmaku)
        //    {
        //        EditorGUI.indentLevel++;
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletcnt"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("intervalShu"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("angleNWay"));
        //        EditorGUI.indentLevel--;
        //        EditorGUILayout.Space();
        //    }

        //    // 4.3 五个一组弹幕圈
        //    showGroupDanmaku = EditorGUILayout.Foldout(showGroupDanmaku, "🎪 五个一组弹幕圈", true);
        //    if (showGroupDanmaku)
        //    {
        //        EditorGUI.indentLevel++;
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("circleCount"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("groupCount"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("groupSpacing"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("circleSpacing"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletSpeed"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("ways"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("inter"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("second"));
        //        EditorGUI.indentLevel--;
        //        EditorGUILayout.Space();
        //    }

        //    // 4.4 随机一圈圆形弹幕
        //    showRandomDanmaku = EditorGUILayout.Foldout(showRandomDanmaku, "🎲 随机圆形弹幕", true);
        //    if (showRandomDanmaku)
        //    {
        //        EditorGUI.indentLevel++;
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletsPerRing"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnRadius"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("ringCount"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("ringInterval"));
        //        EditorGUI.indentLevel--;
        //        EditorGUILayout.Space();
        //    }

        //    // 4.5 螺旋散射
        //    showSpiralDanmaku = EditorGUILayout.Foldout(showSpiralDanmaku, "🌪️ 螺旋散射", true);
        //    if (showSpiralDanmaku)
        //    {
        //        EditorGUI.indentLevel++;
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("spinSpeed1"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("interval1"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletsPerWave"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("sped"));
        //        EditorGUI.indentLevel--;
        //        EditorGUILayout.Space();
        //    }

        //    // 4.6 散射设置
        //    showScatterDanmaku = EditorGUILayout.Foldout(showScatterDanmaku, "💥 散射设置", true);
        //    if (showScatterDanmaku)
        //    {
        //        EditorGUI.indentLevel++;
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletsPerShot"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("aimOneAtPlayer"));
        //        EditorGUI.indentLevel--;
        //        EditorGUILayout.Space();
        //    }

        //    // 4.7 波形发射
        //    showWaveDanmaku = EditorGUILayout.Foldout(showWaveDanmaku, "🌊 波形发射", true);
        //    if (showWaveDanmaku)
        //    {
        //        EditorGUI.indentLevel++;
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletsWavePerShot"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("waveRings"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("waveInterval"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("waveAmplitudeDeg"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("waveFrequency"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("spiralOffsetSpeed"));
        //        EditorGUI.indentLevel--;
        //        EditorGUILayout.Space();
        //    }

        //    // 4.8 陀螺长链高速旋转
        //    showRotateDanmaku = EditorGUILayout.Foldout(showRotateDanmaku, "⚡ 陀螺长链旋转", true);
        //    if (showRotateDanmaku)
        //    {
        //        EditorGUI.indentLevel++;
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("stepDealy"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxRotate"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("minRotate"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateAccel"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateDel"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("LianRota_j1"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("LianRota_timer11"));
        //        EditorGUI.indentLevel--;
        //        EditorGUILayout.Space();
        //    }

        //    // 4.9 陀螺开场高速旋转
        //    showHighSpeedDanmaku = EditorGUILayout.Foldout(showHighSpeedDanmaku, "🚀 开场高速旋转", true);
        //    if (showHighSpeedDanmaku)
        //    {
        //        EditorGUI.indentLevel++;
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("stepRotateHighDealy"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxHighRotate"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("minHighRotate"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateHighAccel"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateHighAccelMax"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateHighAccelAc"));
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateHigh_j1"));
        //        EditorGUI.indentLevel--;
        //        EditorGUILayout.Space();
        //    }

        //    // 4.10 大圆圈散射
        //    EditorGUILayout.LabelField("🎯 大圆圈散射设置", EditorStyles.boldLabel);
        //    EditorGUI.indentLevel++;
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("fireScatterTimePerWave"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("fireScatterTimePerRound"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("fireScatterBulletsPerShot"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("fireScatterTotalRound"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("fireScatter3TimePerWave"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("fireScatter3TimePerRound"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("fireScatter3BulletsPerShot"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("fireScatter3TotalRound"));
        //    EditorGUI.indentLevel--;

        //    EditorGUI.indentLevel--;
        //    EditorGUILayout.Space();
        //}

        //// 5. 死亡掉落/结算
        //showDeathSettings = EditorGUILayout.Foldout(showDeathSettings, "💀 死亡掉落/结算", true);
        //if (showDeathSettings)
        //{
        //    EditorGUI.indentLevel++;
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("dropPrefab"));
        //    EditorGUILayout.PropertyField(serializedObject.FindProperty("leafParticlePrefab"));
        //    EditorGUI.indentLevel--;
        //}

        //serializedObject.ApplyModifiedProperties();

        //// 添加一些调试信息
        //EditorGUILayout.Space();
        //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //EditorGUILayout.LabelField("调试信息", EditorStyles.boldLabel);
        //EditorGUILayout.LabelField($"当前阶段: {Boss.currentPhaseIndex}");
        //EditorGUILayout.LabelField($"无敌状态: {Boss.isInvincible}");
        //if (Boss.isInvincible)
        //{
        //    EditorGUILayout.LabelField($"剩余无敌时间: {Boss.invincibleTime:F1}秒");
        //}
    
}

    private void OnRulesGUI(Boss Boss)
    {
        GUILayout.Label("BossPhase");
        //GUILayout.BeginScrollView(scrollPos);
        GUILayout.BeginVertical();
        for (int i = 0; i < Boss.Phases.Count; i++)
        {
            EditorGUILayout.ObjectField(Boss.Phases[i], typeof(BossPhase), true);

        }
        GUILayout.EndVertical();
        //GUILayout.EndScrollView(scrollPos);
        if (GUILayout.Button("Add Rule"))
        {
            Boss.Phases.Add(new BossPhase());
        }
    }
}