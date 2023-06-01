using Spells;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Spell))]
    public class SpellEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Spell spell = (Spell)target;
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Parameters", EditorStyles.boldLabel);
            
            spell.hasOnHitEffect = EditorGUILayout.Toggle("Has On Hit Effect", spell.hasOnHitEffect);
            if (spell.hasOnHitEffect)
            {
                EditorGUI.indentLevel++;
                spell.onHitEffect = EditorGUILayout.ObjectField("On Hit Effect", spell.onHitEffect, typeof(GameObject)) as GameObject;
                EditorGUI.indentLevel--;
            }

            if (spell.spellType == SpellsType.Projectile)
            {
                DrawProjectileParameters(spell);
            } else if (spell.spellType == SpellsType.Teleport)
            {
                DrawTeleportParameters(spell);
            } else if (spell.spellType == SpellsType.Dash)
            {
                DrawDashParameters(spell);
            } else if (spell.spellType == SpellsType.AoeCast)
            {
                DrawCastParameters(spell);
            }
        }

        private void DrawCastParameters(Spell spell)
        {
            spell.zoneSpell = EditorGUILayout.Toggle("has Zone Duration", spell.zoneSpell);

            if (spell.zoneSpell)
            {
                spell.spellDuration = EditorGUILayout.Slider("Spell Duration", spell.spellDuration, 0, 16);
            }
        
            if (spell.baseDamage > 0)
            {
                spell.damageRadius = EditorGUILayout.Slider("Damage Radius", spell.damageRadius, 0, 8);
                spell.centerOffset = EditorGUILayout.Vector3Field("Center Offset", spell.centerOffset);
            }

            SceneView.RepaintAll();
        }

        private void DrawDashParameters(Spell spell)
        {
            spell.dashDistance = EditorGUILayout.IntField("Dash Distance", spell.dashDistance);
        
            if (spell.baseDamage > 0)
            {
                spell.damageRadius = EditorGUILayout.Slider("Damage Radius", spell.damageRadius, 0, 8);
            }

            SceneView.RepaintAll();
        }
    

        private void OnSceneGUI()
        {
            Spell spell = (Spell)target;
        
        
            if (spell.spellType == SpellsType.Projectile)
            {
            } else if (spell.spellType == SpellsType.Teleport)
            {
                if (spell.baseDamage > 0)
                {
                    Handles.DrawWireDisc(spell.transform.position, Vector3.back, spell.damageRadius);
                }
            } else if (spell.spellType == SpellsType.Dash)
            {
                if (spell.baseDamage > 0)
                {
                    Handles.DrawWireDisc(spell.transform.position, Vector3.back, spell.damageRadius);
                }
            } else if (spell.spellType == SpellsType.AoeCast)
            {
                if (spell.baseDamage > 0)
                {
                    Handles.DrawWireDisc(spell.transform.position + spell.centerOffset, Vector3.back, spell.damageRadius);
                }
            }

        }

        private void DrawTeleportParameters(Spell spell)
        {
            spell.startParticles = EditorGUILayout.ObjectField("Start Particles" ,spell.startParticles, typeof(GameObject), true) as GameObject;
            spell.endParticles = EditorGUILayout.ObjectField("End Particles" ,spell.endParticles, typeof(GameObject), true) as GameObject;
        
            if (spell.baseDamage > 0)
            {
                spell.damageRadius = EditorGUILayout.Slider("Damage Radius", spell.damageRadius, 0, 8);
            }

            SceneView.RepaintAll();
        
        }

        private static void DrawProjectileParameters(Spell spell)
        {
            /* spell.isInfPierce = EditorGUILayout.BeginToggleGroup("Has Inf Piercing", spell.isInfPierce);
        spell.pierce = EditorGUILayout.IntSlider("Pierce Amount", spell.pierce, 1, 256);
        EditorGUILayout.EndToggleGroup(); */
        
        
            spell.destroyOnAnimEnd = EditorGUILayout.Toggle("Destroy on Animation End", spell.destroyOnAnimEnd);
            spell.isInfPierce = EditorGUILayout.Toggle("Has Inf Piercing", spell.isInfPierce);
        
            if (!spell.isInfPierce)
            {
                EditorGUI.indentLevel++;
                spell.pierce = EditorGUILayout.IntSlider("Pierce Amount", spell.pierce, 1, 256);
                EditorGUI.indentLevel--;
            }
        
           
            spell.phantom = EditorGUILayout.Toggle("Passes through Walls", spell.phantom);

        }
    }
}
