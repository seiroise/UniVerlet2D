using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace UniVerlet2D {

	[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
	internal sealed class MinMaxRangeAttrDrawer : PropertyDrawer {

		private const int NUM_WIDTH = 50;
		private const int PADDING = 5;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			using (new EditorGUI.PropertyScope(position, label, property)) {
				MinMaxRangeAttribute att = (MinMaxRangeAttribute)attribute;

				if (property.type.Equals("MinMax")) {

					EditorGUI.BeginProperty(position, label, property);

					position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

					Rect minRect = new Rect(position.x, position.y, NUM_WIDTH, position.height);
					Rect sliderRect = new Rect(minRect.x + minRect.width + PADDING, position.y, position.width - (NUM_WIDTH + PADDING) * 2, position.height);
					Rect maxRect = new Rect(sliderRect.x + sliderRect.width + PADDING, position.y, NUM_WIDTH, position.height);

					SerializedProperty minProp = property.FindPropertyRelative("_min");
					SerializedProperty maxProp = property.FindPropertyRelative("_max");
					float min = minProp.floatValue;
					float max = maxProp.floatValue;
					min = Mathf.Clamp(EditorGUI.FloatField(minRect, min), att.minLimit, max);
					max = Mathf.Clamp(EditorGUI.FloatField(maxRect, max), min, att.maxLimit);
					EditorGUI.MinMaxSlider(sliderRect, ref min, ref max, att.minLimit, att.maxLimit);
					minProp.floatValue = min;
					maxProp.floatValue = max;

					EditorGUI.EndProperty();

				} else {
					EditorGUI.PropertyField(position, property, label);
				}
			}
		}
	}
}