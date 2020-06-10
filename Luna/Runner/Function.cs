﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Luna.Types;
using Luna.Assets;
using OpenTK.Graphics.OpenGL;

namespace Luna.Runner {
    [AttributeUsage(AttributeTargets.Method)]
    public class FunctionDefinition : Attribute {
        public string Name;

        public FunctionDefinition(string _name) {
            this.Name = _name;
        }

        public static void Initalize() {
            MethodInfo[] _functionGet = typeof(Function).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach(MethodInfo _functionHandler in _functionGet) {
                Function.Mapping.Add(_functionHandler.GetCustomAttribute<FunctionDefinition>().Name,
                    (Function.Handler)Delegate.CreateDelegate(typeof(Function.Handler), _functionHandler)
                );
            }
        }
    }

    static class Function {
        public delegate LValue Handler(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack);
        public static Dictionary<string, Handler> Mapping = new Dictionary<string, Handler>();

        [FunctionDefinition("event_inherited")]
        public static LValue event_inherited(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            //
            return LValue.Real(0);
        }

        #region Instances
        [FunctionDefinition("instance_create_depth")]
        public static LValue instance_create_depth(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            LInstance _instCreate = new LInstance(_assets.InstanceList, _assets.ObjectMapping[(int)(double)_arguments[2].Value], (double)_arguments[0].Value, (double)_arguments[1].Value);
            if (_instCreate.PreCreate != null) _instCreate.Environment.ExecuteCode(_assets, _instCreate.PreCreate);
            if (_instCreate.Create != null) _instCreate.Environment.ExecuteCode(_assets, _instCreate.Create);
            return LValue.Real(_instCreate.ID);
        }

        [FunctionDefinition("instance_destroy")]
        public static LValue instance_destroy(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            _assets.InstanceList.Remove(_environment.Instance);
            return LValue.Real(0);
        }
        #endregion

        #region Input
        [FunctionDefinition("keyboard_check")]
        public static LValue keyboard_check(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            return LValue.Real((double)(Input.KeyCheck(_arguments[0]) == true ? 1 : 0));
        }

        [FunctionDefinition("keyboard_check_pressed")]
        public static LValue keyboard_check_pressed(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            return new LValue(LType.Number, (double)(Input.KeyPressed(_arguments[0]) == true ? 1 : 0));
        }

        [FunctionDefinition("keyboard_check_released")]
        public static LValue keyboard_check_released(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            return new LValue(LType.Number, (double)(Input.KeyReleased(_arguments[0]) == true ? 1 : 0));
        }
        #endregion

        #region Rendering

        [FunctionDefinition("draw_set_circle_precision")]
        public static LValue draw_set_circle_precision(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack)
        {
            _assets.CirclePrecision = (int) _arguments[0].Number;
            return LValue.Real(0);
        }
        
        [FunctionDefinition("draw_circle")]
        public static LValue draw_circle(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            GL.Begin(PrimitiveType.TriangleFan);

            double _x = ((double)_arguments[0].Value / (_assets.RoomWidth / 2));
            double _y = -((double)_arguments[1].Value / (_assets.RoomHeight / 2));
            double _r = (double)_arguments[2].Value / _assets.CirclePrecision;

            GL.Vertex2(_x, _y);
            for(int i = 0; i <= 360; i+=360/_assets.CirclePrecision) {
                GL.Vertex2(_x + (Math.Cos(i*(Math.PI/180)) * _r), _y + (Math.Sin(i*(Math.PI/180)) * _r));
            }

            GL.End();
            return LValue.Real(0);
        }
        #endregion

        #region Math
        [FunctionDefinition("max")]
        public static LValue max(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            double[] _val = new double[_count];
            for (int i = 0; i < _count; i++) _val[i] = (double)_arguments[i];
            return LValue.Real(_val.Max());
            /*double[] _val = new double[_count];
            for (int i = 0; i < _count; i++) _val[i] = _stack.Pop();
            _stack.Push(new LValue(LType.Number, _val.Max()));*/
        }

        [FunctionDefinition("min")]
        public static LValue min(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            double[] _val = new double[_count];
            for (int i = 0; i < _count; i++) _val[i] = (double)_arguments[i];
            return LValue.Real(_val.Min());
        }

        [FunctionDefinition("lengthdir_x")]
        public static LValue lengthdir_x(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            double _len = (double)_arguments[0].Value;
            double _dir = (double)_arguments[1].Value;

            double _Px = (_len * Math.Cos(_dir * Math.PI / 180));
            double _o81 = Math.Round(_Px);
            double _F6 = _Px - _o81;
            if (Math.Abs(_F6) < 0.0001) {
                return LValue.Real(_o81);
            } else {
                return LValue.Real(_Px);
            }
            
        }

        [FunctionDefinition("lengthdir_y")]
        public static LValue lengthdir_y(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            double _len = (double)_arguments[0].Value;
            double _dir = (double)_arguments[1].Value;

            double _Px = (_len * Math.Sin(_dir * Math.PI / 180));
            double _o81 = Math.Round(_Px);
            double _F6 = _Px - _o81;
            if (Math.Abs(_F6) < 0.0001) {
                return new LValue(LType.Number, _o81);
            } else {
                return new LValue(LType.Number, _Px);
            }
        }

        [FunctionDefinition("irandom")]
        public static LValue irandom(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            return LValue.Real((double)_assets.RandomGen.Next(0, (int)(double)_arguments[0].Value));
        }
        #endregion

        #region Arrays
        [FunctionDefinition("array_create")]
        public static LValue array_create(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            double _arrayLength = (double)_arguments[0].Value;
            LValue _valArray = new LValue(LType.Array, new LValue[(int)_arrayLength]);
            LValue _valDefault = (_arguments.Length > 1 ? _arguments[1] : new LValue(LType.Number, (double)0));
            for(int i = 0; i < _arrayLength; i++) {
                _valArray.Array[i] = _valDefault;
            }
            return _valArray;
        }

        [FunctionDefinition("@@NewGMLArray@@")]
        public static LValue NewGMLArray(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            LValue _valArray = new LValue(LType.Array, new LValue[_arguments.Length]);
            for(int i = 0; i < _arguments.Length; i++) {
                _valArray.Array[i] = _arguments[i];
            }
            return _valArray;
        }

        [FunctionDefinition("array_length")]
        public static LValue array_length(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            if (_arguments[0].Type == LType.Array) {
                return new LValue(LType.Number, (double)_arguments[0].Array.Length);
            }
            return new LValue(LType.Number, (double)0);
        }
        #endregion

        #region Types
        [FunctionDefinition("string")]
        public static LValue _string(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            return new LValue(LType.String, _arguments[0].Value.ToString());
        }
        #endregion

        #region Runner
        [FunctionDefinition("show_message")]
        public static LValue show_message(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            MessageBox.Show((string)_arguments[0], _assets.DisplayName, MessageBoxButtons.OK);
            return LValue.Real(0);
        }

        [FunctionDefinition("show_debug_message")]
        public static LValue show_debug_message(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            Console.WriteLine(_arguments[0].Value);
            return LValue.Real(0);
        }

        [FunctionDefinition("parameter_count")]
        public static LValue parameter_count(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            return LValue.Real(Program.Arguments.Length);
        }

        [FunctionDefinition("parameter_string")]
        public static LValue parameter_string(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            int _paramIndex = (int)(double)_arguments[0];
            if (_paramIndex >= 0 && _paramIndex < Program.Arguments.Length) {
                return LValue.Text(Program.Arguments[_paramIndex]);
            }
            return LValue.Text("");
        }
        #endregion

        #region Assets - Room
        [FunctionDefinition("room_goto")]
        public static LValue room_goto(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            return LValue.Real(0);
        }

        [FunctionDefinition("room_get_name")]
        public static LValue room_get_name(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            int _roomGet = (int)(double)_arguments[0];
            if (_roomGet >= 0 && _roomGet < _assets.RoomMapping.Count) {
                return new LValue(LType.String, _assets.RoomMapping[_roomGet].Name);
            }
            return new LValue(LType.Number, (double)0); // TODO: this needs to be undefined
        }
        
        [FunctionDefinition("room_get_viewport")]
        public static LValue room_get_viewport(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            int _roomGet = (int)(double)_arguments[0];
            int _viewGet = (int)(double)_arguments[1];
            if (_roomGet >= 0 && _roomGet < _assets.RoomMapping.Count && _viewGet >= 0 && _viewGet < 8)
            {
                LRoomView _view = _assets.RoomMapping[_roomGet].Views[_viewGet];
                return LValue.Values(_view.Enabled,_view.X,_view.Y,_view.Width,_view.Height);
            }
            return LValue.Real(0);
        }
        #endregion
        
        #region Assets - Object

        [FunctionDefinition("object_exists")]
        public static LValue object_exists(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack){
            int _objGet = (int)(double)_arguments[0];
            if (_objGet >= 0 && _objGet < _assets.ObjectMapping.Count) {
                return LValue.Real(1);
            }
            return LValue.Real(0);
        }
        
        [FunctionDefinition("object_get_name")]
        public static LValue object_get_name(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack) {
            int _objGet = (int)(double)_arguments[0];
            if (_objGet >= 0 && _objGet < _assets.ObjectMapping.Count) {
                return LValue.Text(_assets.ObjectMapping[_objGet].Name);
            }
            return LValue.Real(0);
        }
        
        [FunctionDefinition("object_get_mask")]
        public static LValue object_get_mask(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack){
            int _objGet = (int)(double)_arguments[0];
            if (_objGet >= 0 && _objGet < _assets.ObjectMapping.Count)
            {
                LObject _obj = _assets.ObjectMapping[_objGet];
                if (_obj.Mask != null) return LValue.Real(_obj.Mask.Index);
            }
            return LValue.Real(-1);
        }
        
        [FunctionDefinition("object_get_parent")]
        public static LValue object_get_parent(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack){
            int _objGet = (int)(double)_arguments[0];
            if (_objGet >= 0 && _objGet < _assets.ObjectMapping.Count)
            {
                LObject _obj = _assets.ObjectMapping[_objGet];
                if (_obj.Parent != null) return LValue.Real(_obj.Parent.Index);
            }
            //docs say return -1 if object doesn't exist or found object doesn't have a parent
            return LValue.Real(-1);
        }
        
        [FunctionDefinition("object_get_persistent")]
        public static LValue object_get_persistent(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack){
            int _objGet = (int)(double)_arguments[0];
            if (_objGet >= 0 && _objGet < _assets.ObjectMapping.Count)
            {
                LObject _obj = _assets.ObjectMapping[_objGet];
                return LValue.Real(_obj.Persistent ? 1 : 0);
            }
            return LValue.Real(0);
        }
        
        [FunctionDefinition("object_get_physics")]
        public static LValue object_get_physics(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack){
            int _objGet = (int)(double)_arguments[0];
            if (_objGet >= 0 && _objGet < _assets.ObjectMapping.Count)
            {
                LObject _obj = _assets.ObjectMapping[_objGet];
                return LValue.Real(_obj.PhysObject ? 1 : 0);
            }
            return LValue.Real(0);
        }
        
        [FunctionDefinition("object_get_solid")]
        public static LValue object_get_solid(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack){
            int _objGet = (int)(double)_arguments[0];
            if (_objGet >= 0 && _objGet < _assets.ObjectMapping.Count)
            {
                LObject _obj = _assets.ObjectMapping[_objGet];
                return LValue.Real(_obj.Solid ? 1 : 0);
            }
            return LValue.Real(0);
        }
        
        [FunctionDefinition("object_get_sprite")]
        public static LValue object_get_sprite(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack){
            int _objGet = (int)(double)_arguments[0];
            if (_objGet >= 0 && _objGet < _assets.ObjectMapping.Count)
            {
                LObject _obj = _assets.ObjectMapping[_objGet];
                if (_obj.Sprite != null) return LValue.Real(_obj.Sprite.Index);
            }
            return LValue.Real(-1);
        }
        
        [FunctionDefinition("object_get_visible")]
        public static LValue object_get_visible(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack){
            int _objGet = (int)(double)_arguments[0];
            if (_objGet >= 0 && _objGet < _assets.ObjectMapping.Count)
            {
                LObject _obj = _assets.ObjectMapping[_objGet];
                return LValue.Real(_obj.Visible ? 1 : 0);
            }
            return LValue.Real(0);
        }
        
        [FunctionDefinition("object_is_ancestor")]
        public static LValue object_is_ancestor(Game _assets, Domain _environment, LValue[] _arguments, Int32 _count, Stack<LValue> _stack){
            int _objGet = (int)(double)_arguments[0];
            int _parGet = (int)(double)_arguments[1];
            if (_objGet >= 0 && _objGet < _assets.ObjectMapping.Count && _parGet >= 0 && _parGet < _assets.ObjectMapping.Count)
            {
                //define the recursive function
                //variables go in Game but where do functions go?
                //will move when I know that 
                Func<LObject, LObject, bool> checkAncestry = null;
                checkAncestry = (_childObj,_parentObj) =>
                {
                    if (_childObj.Parent == _parentObj) return true;
                    bool _isAncestor = false;
                    if (_childObj.Parent != null)
                    {
                        _isAncestor = checkAncestry(_parentObj, _parentObj.Parent);
                    }

                    return _isAncestor;
                };
                LObject _obj = _assets.ObjectMapping[_objGet];
                LObject _par = _assets.ObjectMapping[_objGet];
                if (_obj.Sprite != null) return LValue.Real(checkAncestry(_obj,_par) ? 1 : 0);
            }
            return LValue.Real(0);
        }
        
        #endregion
    }
}
