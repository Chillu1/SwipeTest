using UnityEngine;
using UnityEngine.UIElements;

namespace OktoSwipe
{
	public sealed class ScrollSystem
	{
		private readonly Camera _camera;
		private readonly Scroller _scroller;

		private float _slideBetweenValue;
		private Vector3 _cameraDestination, _nextCameraDestination;

		private bool _slideValueIsInitialized;

		public ScrollSystem(UIDocument uiDocument, Vector3[] cameraPositions)
		{
			_camera = Camera.main;
			_cameraDestination = _camera.transform.position;

			var scrollView = uiDocument.rootVisualElement.Q<ScrollView>();
			_scroller = scrollView.Q<Scroller>();
			_scroller.valueChanged += value =>
			{
				//highValue is not correct before the first scroll
				if (!_slideValueIsInitialized)
				{
					_slideBetweenValue = _scroller.highValue / (scrollView.Q("unity-content-container").childCount - 1);
					_slideValueIsInitialized = true;
				}

				int positionIndex = Mathf.RoundToInt(value / _slideBetweenValue);
				if (positionIndex < cameraPositions.Length)
					_cameraDestination = cameraPositions[positionIndex];
				if (positionIndex + 1 < cameraPositions.Length)
					_nextCameraDestination = cameraPositions[positionIndex + 1];
			};
		}

		public void Update(float deltaTime)
		{
			if (!_slideValueIsInitialized)
				return;

			var wantedDestination = _cameraDestination;
			//TODO destination is jumping because we update the _nextCameraDestination when go past the slide
			wantedDestination.y = Mathf.Lerp(_cameraDestination.y, _nextCameraDestination.y,
				(_scroller.value % _slideBetweenValue) / _slideBetweenValue);
			_camera.transform.position = Vector3.Lerp(_camera.transform.position, wantedDestination, deltaTime * 5f);

			//Slowly move back scroller to the nearest slide
			_scroller.value = Mathf.Lerp(_scroller.value, Mathf.RoundToInt(_scroller.value / _slideBetweenValue) * _slideBetweenValue,
				deltaTime * 5f);
		}
	}
}