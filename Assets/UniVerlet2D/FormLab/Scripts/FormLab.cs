using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVerlet2D.Data;

namespace UniVerlet2D.Lab {

	public class FormLab : MonoBehaviour {

		public enum Mode {
			None, Stop, Play
		}

		public enum EditMode {
			None, Particle, Spring, Angle
		}

		public enum EditMethod {
			Make, Delete, Adjust
		}

		/*
		 * Fields
		 */

		[SerializeField, ReadOnly]
		Mode _mode = Mode.None;
		[SerializeField, ReadOnly]
		EditMode _editMode = EditMode.None;
		[SerializeField, ReadOnly]
		EditMethod _editMethod = EditMethod.Make;

		[Header("Sim")]
		[SerializeField]
		MonoSimulator _monoSim;
		[SerializeField]
		SimRenderer _simRenderer;
		[SerializeField]
		SimInteractionController _simInteraction;

		[Header("Marker")]
		public MarkerManager markerManager;
		public MarkerDetector markerDetector;

		[Header("Form builder")]
		public TreeFormBuilder treeFormBuilder;

		[Header("Guide")]
		public ConnectionGuide connectionGuide;

		[Header("Maker")]
		public SimElemMaker elemMaker;

		[SerializeField]
		CommandStack _commandStack;

		IEditModeOperator _currentEditMode;
		Dictionary<EditMode, IEditModeOperator> _editModeDic;
		RelatedForm _simRelatedForm;

		// play mode
		bool _isDragging = false;
		int _draggedIdx;
		Particle _draggedParticle;

		// editable form
		EditableForm _editableForm;
		string _formattedText;

		/*
		 * Properties
		 */

		public EditMode editMode { get { return _editMode; } }
		public EditMethod editMethod { get { return _editMethod; } }

		/*
		 * Unity events
		 */

		void Awake() {
			_commandStack = new CommandStack();

			_editModeDic = new Dictionary<EditMode, IEditModeOperator>();

			_editModeDic.Add(EditMode.Particle, new ParticleEditModeOperator(this));
			_editModeDic.Add(EditMode.Spring, new SpringEditModeOperator(this));
			_editModeDic.Add(EditMode.Angle, new AngleEditModeOperator(this));

			_editableForm = new EditableForm();
		}

		void Start() {
			
			SwitchModeTo(Mode.Stop);
			SwitchEditModeTo(EditMode.Particle);

			/*
			if(markerManager) {
				var form = treeFormBuilder.Build(Vector2.zero);
				_monoSim.sim.ImportForm(form);
				markerManager.MakeFromSim(_monoSim.sim);
			}
			*/
			// テスト
			// var formText = _monoSim.sim.ExportFormText();
			// Debug.Log(formText.text);
		}

		void Update() {
			if(_mode == Mode.Play) {
				if(!_isDragging) {
					if(Input.GetMouseButtonDown(0)) {
						int idx;
						if(_monoSim.sim.GetOverlapParticle(markerDetector.wmPos, 0.5f, out idx)) {
							_isDragging = true;
							_draggedIdx = idx;
							_draggedParticle = _monoSim.sim.GetParticleAt(idx);
						}
					}
				} else {
					if(Input.GetMouseButtonUp(0)) {
						_isDragging = false;
					} else if(Input.GetMouseButton(0)) {
						_draggedParticle.pos = markerDetector.wmPos;
					}
				}
			} else {
				if(_currentEditMode != null) {
					_currentEditMode.Update();
				}
			}
		}

		/*
		 * State related
		 */

		public void SwitchModeTo(Mode mode) {
			if(_mode == mode) {
				return;
			}
			_mode = mode;
			switch(_mode) {
			case Mode.Play:
				SwitchToPlay();
				break;
			case Mode.Stop:
				SwitchToStop();
				break;
			}
		}

		void SwitchToPlay() {
			_monoSim.enabled = true;
			_simRenderer.enabled = true;

			markerManager.Clear();
			_monoSim.updateSim = true;
			_simRenderer.updateMesh = true;

			_formattedText = _editableForm.ExportFormattedText();
			Debug.Log(_formattedText);

			// _simRelatedForm = _monoSim.sim.ExportRelatedForm();
		}

		void SwitchToStop() {
			_monoSim.enabled = false;
			_simRenderer.enabled = false;

			_monoSim.updateSim = false;
			_simRenderer.Clear();
			_simRenderer.updateMesh = false;

			_editableForm.ImportFormattedText(_formattedText);

			// _monoSim.sim.ImportRelatedForm(_simRelatedForm);

			// markerManager.MakeFromSim(_monoSim.sim);
		}

		public void SwitchEditModeTo(EditMode editMode) {
			if(_editMode == editMode) {
				return;
			}
			_editMode = editMode;
			if(_currentEditMode != null) {
				_currentEditMode.ExitMode();
			}
			_currentEditMode = _editModeDic[editMode];
			if(_currentEditMode != null) {
				_currentEditMode.EnterMode();
			}

			if(elemMaker) {
				switch(_editMode) {
				case EditMode.Particle:
					elemMaker.SetSimElemProfile(SimElemDefine.PARTICLE_ID);
					break;
				case EditMode.Spring:
					elemMaker.SetSimElemProfile(SimElemDefine.SPRING_ID);
					break;
				case EditMode.Angle:
					elemMaker.SetSimElemProfile(SimElemDefine.ANGLE_ID);
					break;
				}
			}
		}

		/*
		 * Operating methods
		 */

		public void Undo() {
			_commandStack.Undo();
		}

		public void Redo() {
			_commandStack.Redo();
		}

		public void MakeParticle(Vector2 pos) {
			_commandStack.ExeCommand(new MakeParticleCommand(_monoSim.sim, markerManager, pos));
		}

		public void DeleteParticle(int uid) {
			_commandStack.ExeCommand(new DeleteParticleCommand(_monoSim.sim, markerManager, uid));
		}

		public void MakeSpring(int aUID, int bUID, float stiffness) {
			_commandStack.ExeCommand(new MakeSpringCommand(_monoSim.sim, markerManager, aUID, bUID, stiffness));
		}

		public void DeleteSpring(int uid) {
			_commandStack.ExeCommand(new DeleteSpringCommand(_monoSim.sim, markerManager, uid));
		}

		public void MakeAngle(int aUID, int bUID, int mUID, float stiffness) {
			_commandStack.ExeCommand(new MakeAngleCommand(_monoSim.sim, markerManager, aUID, bUID, mUID, stiffness));
		}

		public void DeleteAngle(int uid) {
			_commandStack.ExeCommand(new DeleteAngleCommand(_monoSim.sim, markerManager, uid));
		}

		/*
		 * UI Callback
		 */

		public void OnChangeToPlay(bool v) {
			if(v) {
				SwitchModeTo(Mode.Play);
			}
		}

		public void OnChangeToStop(bool v) {
			if(v) {
				SwitchModeTo(Mode.Stop);
			}
		}

		public void OnChangeToParticle(bool v) {
			if(v) {
				SwitchEditModeTo(EditMode.Particle);
			}
		}

		public void OnChangeToSpring(bool v) {
			if(v) {
				SwitchEditModeTo(EditMode.Spring);
			}
		}

		public void OnChangeToAngle(bool v) {
			if(v) {
				SwitchEditModeTo(EditMode.Angle);
			}
		}

		public void OnChangeToMake(bool v) {
			if(v) {
				_editMethod = EditMethod.Make;
			}
		}

		public void OnChangeToDelete(bool v) {
			if(v) {
				_editMethod = EditMethod.Delete;
			}
		}

		public void OnClickUndo() {
			Undo();
		}

		public void OnClickRedo() {
			Redo();
		}

		/*
		 * Sim element callback
		 */

		public void OnMakeSimElemInfo(SimElemInfo simElemInfo) {
			markerManager.MakeSimElemMarker(simElemInfo);
			_editableForm.AddElemInfo(simElemInfo);
		}

		/*
		 * Mouse input
		 */

		public void OnDownSpace(Vector3 pos) {
			if(_currentEditMode != null && _mode == Mode.Stop) {
				// _currentEditMode.DownSpace(pos);

				if(elemMaker) {
					elemMaker.OnDownSpace(pos);
				}
			}
		}

		public void OnDownMarker(SimElemMarker marker) {
			if(_currentEditMode != null && _mode == Mode.Stop) {
				// _currentEditMode.DownMarker(marker);

				if(elemMaker) {
					elemMaker.OnDownMarker(marker);
				}
			}
		}
	}
}