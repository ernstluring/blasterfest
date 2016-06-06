using UnityEngine;
using System.Collections.Generic;

public class XboxControllerMapperWindows : AControlsMapper {

	// Key, Value
	private Dictionary<InputKeyValue, string> _controls;
	private string _systemType = "Windows";

	public XboxControllerMapperWindows (PlayerNumber playerNumber) {
		string playerNumberAndSystem = ((int)playerNumber+1).ToString()+_systemType;
		_controls = new Dictionary<InputKeyValue, string>();
		_controls.Add (InputKeyValue.Horizontal, "Horizontal"+playerNumberAndSystem);
		_controls.Add (InputKeyValue.Vertical, "Vertical"+playerNumberAndSystem);
		_controls.Add (InputKeyValue.Jump, "Jump"+playerNumberAndSystem);
		_controls.Add (InputKeyValue.Jetpack, "Jetpack"+playerNumberAndSystem);
		_controls.Add (InputKeyValue.Shoot, "Shoot"+playerNumberAndSystem);
		_controls.Add (InputKeyValue.AimHorizontal, "AimHorizontal"+playerNumberAndSystem);
		_controls.Add (InputKeyValue.AimVertical, "AimVertical"+playerNumberAndSystem);
		_controls.Add (InputKeyValue.A, "Jump"+playerNumberAndSystem);
		_controls.Add (InputKeyValue.B, "Cancel"+playerNumberAndSystem);
		_controls.Add (InputKeyValue.Start, "Start"+playerNumberAndSystem);
		_controls.Add (InputKeyValue.Select, "Select" + playerNumberAndSystem);
		_controls.Add (InputKeyValue.X, "X"+playerNumberAndSystem);
		_controls.Add (InputKeyValue.Y, "Y"+playerNumberAndSystem);
		_controls.Add (InputKeyValue.DpadHorizontalAxis, "DpadHorizontal" + playerNumberAndSystem);
		_controls.Add (InputKeyValue.DpadVerticalAxis, "DpadVertical" + playerNumberAndSystem);
	}

	public override string GetHorizontalAxis ()
	{
		return _controls[InputKeyValue.Horizontal];
	}

	public override string GetVerticalAxis ()
	{
		return _controls[InputKeyValue.Vertical];
	}

	public override string GetShootTrigger ()
	{
		return _controls[InputKeyValue.Shoot];
	}

	public override string GetJetpackTrigger ()
	{
		return _controls[InputKeyValue.Jetpack];
	}

	public override string GetAimHorizontalAxis ()
	{
		return _controls[InputKeyValue.AimHorizontal];
	}

	public override string GetAimVerticalAxis ()
	{
		return _controls[InputKeyValue.AimVertical];
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

	public override string GetXButton ()
	{
		return _controls [InputKeyValue.X];
	}

	public override string GetYButton ()
	{
		return _controls [InputKeyValue.Y];
	}

	public override string GetStartButton ()
	{
		return _controls[InputKeyValue.Start];
	}

	public override string GetSelectButton ()
	{
		return _controls [InputKeyValue.Select];
	}

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

	public override bool AimingRight () {
		return Input.GetAxisRaw(GetAimHorizontalAxis()) > 0.6f;
	}
	public override bool AimingLeft () {
		return Input.GetAxisRaw(GetAimHorizontalAxis()) < -0.6f;
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
}
