using UnityEngine;
using System.Collections.Generic;

public class KeyboardControlsMapper : AControlsMapper {

	// Key, Value
	private Dictionary<InputKeyValue, string> _controls;

	public KeyboardControlsMapper (PlayerNumber playerNumber) {
		_controls = new Dictionary<InputKeyValue, string>();
		_controls.Add (InputKeyValue.Horizontal, "Horizontal");
		_controls.Add (InputKeyValue.Vertical, "Vertical");
		_controls.Add (InputKeyValue.Shoot, "Fire1");
		_controls.Add (InputKeyValue.AimHorizontal, "Mouse X");
		_controls.Add (InputKeyValue.AimVertical, "Mouse Y");
		_controls.Add (InputKeyValue.Jetpack, "Jump");
		_controls.Add (InputKeyValue.Jump, "Jump");
	}

	public override string GetHorizontalAxis ()
	{
		return _controls[InputKeyValue.Horizontal];
	}

	public override string GetVerticalAxis ()
	{
		return _controls[InputKeyValue.Vertical];
	}

	public override string GetAimHorizontalAxis ()
	{
		return _controls[InputKeyValue.AimHorizontal];
	}

	public override string GetAimVerticalAxis ()
	{
		return _controls[InputKeyValue.AimVertical];
	}

	public override string GetJetpackTrigger ()
	{
		return _controls[InputKeyValue.Jetpack];
	}

	public override string GetShootTrigger ()
	{
		return _controls[InputKeyValue.Shoot];
	}

	public override string GetJumpButton ()
	{
		return _controls[InputKeyValue.Jump];
	}

	public override string GetAButton ()
	{
		return _controls[InputKeyValue.A];
	}

	public override string GetBButton ()
	{
		return _controls[InputKeyValue.B];
	}

	public override string GetStartButton ()
	{
		throw new System.NotImplementedException ();
	}

	public override bool AimingRight () {
		return Input.GetAxisRaw(GetAimHorizontalAxis()) > 5;
	}
	public override bool AimingLeft () {
		return Input.GetAxisRaw(GetAimHorizontalAxis()) < -5;
	}

	// CHANGE THIS!
	public override string GetDpadLeft ()
	{
		return _controls [InputKeyValue.DpadHorizontalAxis];
	}
	public override string GetDpadRight ()
	{
		return _controls [InputKeyValue.DpadHorizontalAxis];
	}
	public override string GetDpadUp ()
	{
		return _controls [InputKeyValue.DpadVerticalAxis];
	}
	public override string GetDpadDown ()
	{
		return _controls [InputKeyValue.DpadVerticalAxis];
	}

	public override bool DpadDown () {
		return Input.GetAxisRaw (GetDpadUp ()) < 0;
	}
	public override bool DpadUp ()
	{
		return Input.GetAxisRaw (GetDpadUp ()) > 0;
	}
	public override bool DpadLeft ()
	{
		return Input.GetAxisRaw (GetDpadLeft ()) < 0;
	}
	public override bool DpadRight ()
	{
		return Input.GetAxisRaw (GetDpadLeft ()) > 0;
	}
	public override string GetXButton ()
	{
		return _controls [InputKeyValue.X];
	}

	public override string GetYButton ()
	{
		return _controls [InputKeyValue.Y];
	}
	public override string GetSelectButton ()
	{
		return _controls [InputKeyValue.Select];
	}
}
