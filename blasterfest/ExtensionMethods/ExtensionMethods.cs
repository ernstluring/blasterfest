using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

namespace Game.Extensions {
	public static class ExtensionMethods {

		public static void SetAlpha(this SpriteRenderer spriteRenderer, float alpha) {
			Color buffered = spriteRenderer.color;
			buffered.a = alpha;
			spriteRenderer.color = buffered;
		}

		public static void SetAlpha(this Image image, float alpha) {
			Color buffered = image.color;
			buffered.a = alpha;
			image.color = buffered;
		}

		public static void SetAnchorX(this RectTransform rectTransform, float x) {
			Vector2 anchorPosition = rectTransform.anchoredPosition;
			anchorPosition.x = x;
			rectTransform.anchoredPosition = anchorPosition;
		}

		public static void SetAnchorY(this RectTransform rectTransform, float y) {
			Vector2 anchorPosition = rectTransform.anchoredPosition;
			anchorPosition.y = y;
			rectTransform.anchoredPosition = anchorPosition;
		}

		public static void SetWidth(this RectTransform rectTransform, float width) {
			Vector2 sizeDelta = rectTransform.sizeDelta;
			sizeDelta.x = width;
			rectTransform.sizeDelta = sizeDelta;
		}

		public static void SetHeight(this RectTransform rectTransform, float height) {
			Vector2 sizeDelta = rectTransform.sizeDelta;
			sizeDelta.y = height;
			rectTransform.sizeDelta = sizeDelta;
		}

		public static void SetPositionX(this Transform transform, float x) {
			Vector3 position = transform.position;
			position.x = x;
			transform.position = position;
		}

		public static void SetPositionY(this Transform transform, float y) {
			Vector3 position = transform.position;
			position.y = y;
			transform.position = position;
		}

		public static void SetPositionZ(this Transform transform, float z) {
			Vector3 position = transform.position;
			position.z = z;
			transform.position = position;
		}

		public static void SetScale(this Transform transform, float amount)
		{
			Vector3 scale = transform.localScale;
			scale.x = scale.y = scale.z = amount;
			transform.localScale = scale;
		}

		public static void SetLocalScaleX(this Transform transform, float amount)
		{
			Vector3 scale = transform.localScale;
			scale.x = amount;
			transform.localScale = scale;
		}

		public static void SetLocalPositionX(this Transform transform, float x) {
			Vector3 position = transform.localPosition;
			position.x = x;
			transform.localPosition = position;
		}

		public static void SetLocalPositionY(this Transform transform, float y) {
			Vector3 position = transform.localPosition;
			position.y = y;
			transform.localPosition = position;
		}

		public static void SetLocalPositionZ(this Transform transform, float z) {
			Vector3 position = transform.localPosition;
			position.z = z;
			transform.localPosition = position;
		}

		public static void ResetTransform(this Transform transform) {
			transform.localPosition = Vector3.zero;
			transform.localScale = Vector3.one;
			transform.rotation = Quaternion.Euler(Vector3.zero);
		}

		public static void SetRigidbody2DVelocity (this Rigidbody2D rb, float x, float y) {
			Vector2 value = rb.velocity;
			value.x = x;
			value.y = y;
			rb.velocity = value;
		}

		public static void SetRigidbody2DVelocityX (this Rigidbody2D rb, float x) {
			Vector2 value = rb.velocity;
			value.x = x;
			rb.velocity = value;
		}

		public static void SetRigidbodyVelocity2DY (this Rigidbody2D rb, float y) {
			Vector2 value = rb.velocity;
			value.y = y;
			rb.velocity = value;
		}

		public static void EnableEmission (this ParticleSystem particleSystem, bool enabled) {
			var emission = particleSystem.emission;
			emission.enabled = enabled;
		}

		public static float GetEmissionRate (this ParticleSystem particleSystem) {
			return particleSystem.emission.rate.constantMax;
		}

		public static void SetEmissionRate (this ParticleSystem particleSystem, float emissionRate) {
			var emission = particleSystem.emission;
			var rate = emission.rate;
			rate.constantMax = emissionRate;
			emission.rate = rate;
		}

		public static Vector3 ScreenPosition(this Transform transform) {
			Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
			return screenPos;
		}

		public static bool IsOutOfScreen(this Transform transform) {
			Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
			return Mathf.Abs(screenPos.x) > Screen.width ||
				Mathf.Abs(screenPos.y) > Screen.height;
		}
	}
}