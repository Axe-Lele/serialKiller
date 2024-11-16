using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MonoBehaviour), true)]
public class MonoBehaviourEditor : Editor
{
	static bool GetComponenets(MonoBehaviour behaviour, object customAttribute, Type elementType, out object components)
	{
		if (customAttribute is GetComponentAttribute)
		{
			var getter = typeof(MonoBehaviour)
							.GetMethod("GetComponents", new Type[0])
							.MakeGenericMethod(elementType);
			components = getter.Invoke(behaviour, null);
		}
		else if (customAttribute is GetComponentInChildrenAttribute)
		{
			var getter = typeof(MonoBehaviour)
							.GetMethod("GetComponentsInChildren", new Type[] { typeof(bool) })
							.MakeGenericMethod(elementType);
			components = getter.Invoke(behaviour,
							new object[] { ((GetComponentInChildrenAttribute)customAttribute).includeInactive });
		}
		else if (customAttribute is GetComponentInParentAttribute)
		{
			var getter = typeof(MonoBehaviour)
							.GetMethod("GetComponentsInParent", new Type[] { typeof(bool) })
							.MakeGenericMethod(elementType);
			components = getter.Invoke(behaviour,
							new object[] { ((GetComponentInParentAttribute)customAttribute).includeInactive });
		}
		else
		{
			components = null;
			return false;
		}

		return true;
	}

	public override void OnInspectorGUI()
	{
		MonoBehaviour behaviour = (MonoBehaviour)target;
		var fields = behaviour.GetType().GetFields(BindingFlags.Public |
													BindingFlags.NonPublic |
													BindingFlags.Instance);

		foreach (var field in fields)
		{
			var customAttributes = field.GetCustomAttributes(true);

			foreach (var customAttribute in customAttributes)
			{
				var type = field.FieldType;

				if (type.IsArray)
				{
					object components;
					if (GetComponenets(behaviour, customAttribute, type.GetElementType(), out components))
						field.SetValue(behaviour, components);
				}
				else if (type.IsGenericType)
				{
					if (type.GetGenericTypeDefinition() == typeof(List<>))
					{
						object components;
						if (GetComponenets(behaviour, customAttribute, type.GetGenericArguments()[0], out components))
							field.SetValue(behaviour, Activator.CreateInstance(type, components));
					}
				}
				else
				{
					if (customAttribute is GetComponentAttribute)
						field.SetValue(behaviour, behaviour.GetComponent(type));
					else if (customAttribute is GetComponentInChildrenAttribute)
						field.SetValue(behaviour, behaviour.GetComponentInChildren(type,
													((GetComponentInChildrenAttribute)customAttribute).includeInactive));
					else if (customAttribute is GetComponentInParentAttribute)
						field.SetValue(behaviour, behaviour.GetComponentInParent(type));
				}
			}
		}

		base.OnInspectorGUI();
	}
}
#endif

[AttributeUsage(AttributeTargets.Field)]
public class GetComponentAttribute : ReadOnlyAttribute
{
}

[AttributeUsage(AttributeTargets.Field)]
public class GetComponentInChildrenAttribute : ReadOnlyAttribute
{
	public readonly bool includeInactive;

	/// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
	public GetComponentInChildrenAttribute(bool includeInactive = false)
	{
		this.includeInactive = includeInactive;
	}
}

[AttributeUsage(AttributeTargets.Field)]
public class GetComponentInParentAttribute : ReadOnlyAttribute
{
	public readonly bool includeInactive;

	/// <param name="includeInactive">Should inactive Components be included in the found set? (Array and List only)</param>
	public GetComponentInParentAttribute(bool includeInactive = false)
	{
		this.includeInactive = includeInactive;
	}
}