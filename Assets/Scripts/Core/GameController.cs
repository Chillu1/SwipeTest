using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace OktoSwipe
{
	public sealed class GameController : MonoBehaviour
	{
		[NonSerialized]
		public Transform[] ContentSlides;

		[NonSerialized]
		public Vector3[] CameraPositions;

		public UIDocument UIDocument;

		private ScrollSystem _scrollSystem;

		private void Start()
		{
			var contentSlidesTransform = GameObject.Find("ContentSlides").transform;
			int contentSlidesCount = contentSlidesTransform.childCount;

			ContentSlides = new Transform[contentSlidesCount];
			CameraPositions = new Vector3[contentSlidesCount];
			for (int i = 0; i < contentSlidesCount; i++)
			{
				var child = contentSlidesTransform.GetChild(i);
				ContentSlides[i] = child;
				CameraPositions[i] = new Vector3(child.transform.position.x, child.transform.position.y + 2.5f, -6);
				UIDocument.rootVisualElement.Q("unity-content-container").hierarchy.Add(new VisualElement());
			}

			_scrollSystem = new ScrollSystem(UIDocument, CameraPositions);
		}

		private void Update()
		{
			_scrollSystem.Update(Time.deltaTime);
		}
	}
}