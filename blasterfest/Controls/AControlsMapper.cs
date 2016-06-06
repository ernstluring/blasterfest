using UnityEngine;

public enum InputKeyValue { Horizontal, Vertical, Jetpack, AimHorizontal, AimVertical, Shoot, Jump, A, B, Start, Select,
							X, Y, DpadRight, DpadLeft, DpadUp, DpadDown, DpadHorizontalAxis, DpadVerticalAxis }
public abstract class AControlsMapper {

	public abstract string GetHorizontalAxis();
	public abstract string GetVerticalAxis();
	public abstract string GetShootTrigger();
	public abstract string GetJetpackTrigger();
	public abstract string GetAimHorizontalAxis();
	public abstract string GetAimVerticalAxis();
	public abstract string GetJumpButton();
	public abstract string GetAButton();
	public abstract string GetBButton();
	public abstract string GetXButton();
	public abstract string GetYButton();
	public abstract string GetStartButton();
	public abstract string GetSelectButton();
	public abstract string GetDpadRight();
	public abstract string GetDpadLeft();
	public abstract string GetDpadUp();
	public abstract string GetDpadDown();

	public abstract bool AimingRight();
	public abstract bool AimingLeft();

	public abstract bool DpadLeft();
	public abstract bool DpadRight();
	public abstract bool DpadUp();
	public abstract bool DpadDown();

	public bool PressedRight () {
		return Input.GetAxisRaw(GetHorizontalAxis()) > 0.3f;
	}
	public bool PressedLeft () {
		return Input.GetAxisRaw(GetHorizontalAxis()) < -0.3f;
	}
	public bool PressedUp () {
		return Input.GetAxisRaw(GetVerticalAxis()) > 0;
	}
	public bool PressedDown () {
		return Input.GetAxisRaw(GetVerticalAxis()) < -0.9f;
	}
	public bool PressedJump () {
		return Input.GetButtonDown(GetJumpButton());
	}
	public bool JetpackTrigger () {
		return Input.GetAxisRaw(GetJetpackTrigger()) > 0;
	}
	public bool ShootTrigger () {
		return Input.GetAxisRaw(GetShootTrigger()) > 0;
	}
	public bool ShootTriggerPressed () {
		return Input.GetAxisRaw(GetShootTrigger()) > 0.9f;
	}
	public bool ShootTriggerReleased () {
		return Input.GetAxisRaw(GetShootTrigger()) < 0.1f;
	}
	public bool PressedAButton () {
		return Input.GetButtonDown(GetAButton());
	}
	public bool PressedBButton () {
		return Input.GetButtonDown(GetBButton());
	}
	public bool PressedXButton () {
		return Input.GetButtonDown (GetXButton ());
	}
	public bool PressedStartButton () {
		return Input.GetButtonDown(GetStartButton());
	}
}
