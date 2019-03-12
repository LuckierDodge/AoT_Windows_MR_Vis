﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
// ###
// # Kindly borrowed and extended from Microsoft MRTK (https://github.com/Microsoft/MixedRealityToolkit-Unity) to work with VRTK.
// ###

using UnityEngine;
#if UNITY_WSA
#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR.WSA.Input;
#else
using UnityEngine.VR.WSA.Input;
#endif
#endif

namespace VRTK.WindowsMixedReality
{
    /// <summary>
    /// This script keeps track of the GameObjects for each button on the controller.
    /// It also keeps track of the animation Transforms in order to properly animate according to user input.
    /// </summary>
    public class MotionControllerInfo
    {
        public readonly GameObject ControllerParent;

#if UNITY_WSA && UNITY_2017_2_OR_NEWER
        public readonly InteractionSourceHandedness Handedness;
#endif

        private GameObject home;
        private Transform homePressed;
        private Transform homeUnpressed;
        private GameObject menu;
        private Transform menuPressed;
        private Transform menuUnpressed;
        private GameObject grasp;
        private Transform graspPressed;
        private Transform graspUnpressed;
        private GameObject thumbstickPress;
        private Transform thumbstickPressed;
        private Transform thumbstickUnpressed;
        private GameObject thumbstickX;
        private Transform thumbstickXMin;
        private Transform thumbstickXMax;
        private GameObject thumbstickY;
        private Transform thumbstickYMin;
        private Transform thumbstickYMax;
        private GameObject select;
        private Transform selectPressed;
        private Transform selectUnpressed;
        private GameObject touchpadPress;
        private Transform touchpadPressed;
        private Transform touchpadUnpressed;
        private GameObject touchpadTouchX;
        private Transform touchpadTouchXMin;
        private Transform touchpadTouchXMax;
        private GameObject touchpadTouchY;
        private Transform touchpadTouchYMin;
        private Transform touchpadTouchYMax;
        private GameObject touchpadTouchVisualizer;
        private GameObject pointingPose;

        private string homePath;
        private string selectPath;
        private string graspPath;
        private string menuPath;
        private string touchpadPath;
        private string thumbstickPath;

        // These values are used to determine if a button's state has changed.
        private bool wasGrasped;
        private bool wasMenuPressed;
        private bool wasHomePressed;
        private bool wasThumbstickPressed;
        private bool wasTouchpadPressed;
        private bool wasTouchpadTouched;
        private Vector2 lastThumbstickPosition;
        private Vector2 lastTouchpadPosition;
        private double lastSelectPressedAmount;

        public MotionControllerInfo(GameObject controllerParent
#if UNITY_WSA && UNITY_2017_2_OR_NEWER
            , InteractionSourceHandedness handedness
#endif
            )
        {
            ControllerParent = controllerParent;
#if UNITY_WSA && UNITY_2017_2_OR_NEWER
            Handedness = handedness;
#endif
        }

        public enum ControllerElementEnum
        {
            // Controller button elements
            Home,
            Menu,
            Grasp,
            Thumbstick,
            Select,
            Touchpad,
            // Controller body elements & poses
            PointingPose
        }

        public bool TryGetElement(ControllerElementEnum element, out Transform elementTransform)
        {
            switch (element)
            {
                // control elements
                case ControllerElementEnum.Home:
                    if (home != null)
                    {
                        elementTransform = home.transform;
                        return true;
                    }
                    break;
                case ControllerElementEnum.Menu:
                    if (menu != null)
                    {
                        elementTransform = menu.transform;
                        return true;
                    }
                    break;
                case ControllerElementEnum.Select:
                    if (select != null)
                    {
                        elementTransform = select.transform;
                        return true;
                    }
                    break;
                case ControllerElementEnum.Grasp:
                    if (grasp != null)
                    {
                        elementTransform = grasp.transform;
                        return true;
                    }
                    break;
                case ControllerElementEnum.Thumbstick:
                    if (thumbstickPress != null)
                    {
                        elementTransform = thumbstickPress.transform;
                        return true;
                    }
                    break;
                case ControllerElementEnum.Touchpad:
                    if (touchpadPress != null)
                    {
                        elementTransform = touchpadPress.transform;
                        return true;
                    }
                    break;
                // body elements & poses
                case ControllerElementEnum.PointingPose:
                    if (pointingPose != null)
                    {
                        elementTransform = pointingPose.transform;
                        return true;
                    }
                    break;
            }

            elementTransform = null;
            return false;
        }

        /// <summary>
        /// Iterates through the Transform array to find specifically named GameObjects.
        /// These GameObjects specify the animation bounds and the GameObject to modify for button,
        /// thumbstick, and touchpad animation.
        /// </summary>
        /// <param name="childTransforms">The transforms of the glTF model.</param>
        /// <param name="visualizerScript">The script containing references to any objects to spawn.</param>
        public void LoadInfo(Transform[] childTransforms)
        {
            foreach (Transform child in childTransforms)
            {
                // Animation bounds are named in two pairs:
                // pressed/unpressed and min/max. There is also a value
                // transform, which is the transform to modify to
                // animate the interactions. We also look for the
                // touch transform, in order to spawn the touchpadTouched
                // visualizer.
                switch (child.name.ToLower())
                {
                    case "touch":
                        touchpadTouchVisualizer = MotionControllerVisualizer.Instance.SpawnTouchpadVisualizer(child);
                        break;
                    case "pointing_pose":
                        pointingPose = child.gameObject;
                        break;
                    case "pressed":
                        switch (child.parent.name.ToLower())
                        {
                            case "home":
                                homePressed = child;
                                break;
                            case "menu":
                                menuPressed = child;
                                break;
                            case "grasp":
                                graspPressed = child;
                                break;
                            case "select":
                                selectPressed = child;
                                break;
                            case "thumbstick_press":
                                thumbstickPressed = child;
                                break;
                            case "touchpad_press":
                                touchpadPressed = child;
                                break;
                        }
                        break;
                    case "unpressed":
                        switch (child.parent.name.ToLower())
                        {
                            case "home":
                                homeUnpressed = child;
                                break;
                            case "menu":
                                menuUnpressed = child;
                                break;
                            case "grasp":
                                graspUnpressed = child;
                                break;
                            case "select":
                                selectUnpressed = child;
                                break;
                            case "thumbstick_press":
                                thumbstickUnpressed = child;
                                break;
                            case "touchpad_press":
                                touchpadUnpressed = child;
                                break;
                        }
                        break;
                    case "min":
                        switch (child.parent.name.ToLower())
                        {
                            case "thumbstick_x":
                                thumbstickXMin = child;
                                break;
                            case "thumbstick_y":
                                thumbstickYMin = child;
                                break;
                            case "touchpad_touch_x":
                                touchpadTouchXMin = child;
                                break;
                            case "touchpad_touch_y":
                                touchpadTouchYMin = child;
                                break;
                        }
                        break;
                    case "max":
                        switch (child.parent.name.ToLower())
                        {
                            case "thumbstick_x":
                                thumbstickXMax = child;
                                break;
                            case "thumbstick_y":
                                thumbstickYMax = child;
                                break;
                            case "touchpad_touch_x":
                                touchpadTouchXMax = child;
                                break;
                            case "touchpad_touch_y":
                                touchpadTouchYMax = child;
                                break;
                        }
                        break;
                    case "value":
                        switch (child.parent.name.ToLower())
                        {
                            case "home":
                                home = child.gameObject;
                                break;
                            case "menu":
                                menu = child.gameObject;
                                break;
                            case "grasp":
                                grasp = child.gameObject;
                                break;
                            case "select":
                                select = child.gameObject;
                                break;
                            case "thumbstick_press":
                                thumbstickPress = child.gameObject;
                                break;
                            case "thumbstick_x":
                                thumbstickX = child.gameObject;
                                break;
                            case "thumbstick_y":
                                thumbstickY = child.gameObject;
                                break;
                            case "touchpad_press":
                                touchpadPress = child.gameObject;
                                break;
                            case "touchpad_touch_x":
                                touchpadTouchX = child.gameObject;
                                break;
                            case "touchpad_touch_y":
                                touchpadTouchY = child.gameObject;
                                break;
                        }
                        break;
                    case "primitive":
                        CreatePathToTransform(child);
                        break;
                }
            }
        }

        public void AnimateGrasp(bool isGrasped)
        {
            if (grasp != null && graspPressed != null && graspUnpressed != null && isGrasped != wasGrasped)
            {
                SetLocalPositionAndRotation(grasp, isGrasped ? graspPressed : graspUnpressed);
                wasGrasped = isGrasped;
            }
        }

        public void AnimateMenu(bool isMenuPressed)
        {
            if (menu != null && menuPressed != null && menuUnpressed != null && isMenuPressed != wasMenuPressed)
            {
                SetLocalPositionAndRotation(menu, isMenuPressed ? menuPressed : menuUnpressed);
                wasMenuPressed = isMenuPressed;
            }
        }

        public void AnimateHome(bool isHomePressed)
        {
            if (home != null && homePressed != null && homeUnpressed != null && isHomePressed != wasHomePressed)
            {
                SetLocalPositionAndRotation(home, isHomePressed ? homePressed : homeUnpressed);
                wasHomePressed = isHomePressed;
            }
        }

        public void AnimateSelect(float newSelectPressedAmount)
        {
            if (select != null && selectPressed != null && selectUnpressed != null && newSelectPressedAmount != lastSelectPressedAmount)
            {
                select.transform.localPosition = Vector3.Lerp(selectUnpressed.localPosition, selectPressed.localPosition, newSelectPressedAmount);
                select.transform.localRotation = Quaternion.Lerp(selectUnpressed.localRotation, selectPressed.localRotation, newSelectPressedAmount);
                lastSelectPressedAmount = newSelectPressedAmount;
            }
        }

        public void AnimateThumbstick(bool isThumbstickPressed, Vector2 newThumbstickPosition)
        {
            if (thumbstickPress != null && thumbstickPressed != null && thumbstickUnpressed != null && isThumbstickPressed != wasThumbstickPressed)
            {
                SetLocalPositionAndRotation(thumbstickPress, isThumbstickPressed ? thumbstickPressed : thumbstickUnpressed);
                wasThumbstickPressed = isThumbstickPressed;
            }

            if (thumbstickX != null && thumbstickY != null && thumbstickXMin != null && thumbstickXMax != null && thumbstickYMin != null && thumbstickYMax != null && newThumbstickPosition != lastThumbstickPosition)
            {
                Vector2 thumbstickNormalized = (newThumbstickPosition + Vector2.one) * 0.5f;

                thumbstickX.transform.localPosition = Vector3.Lerp(thumbstickXMin.localPosition, thumbstickXMax.localPosition, thumbstickNormalized.x);
                thumbstickX.transform.localRotation = Quaternion.Lerp(thumbstickXMin.localRotation, thumbstickXMax.localRotation, thumbstickNormalized.x);

                thumbstickY.transform.localPosition = Vector3.Lerp(thumbstickYMax.localPosition, thumbstickYMin.localPosition, thumbstickNormalized.y);
                thumbstickY.transform.localRotation = Quaternion.Lerp(thumbstickYMax.localRotation, thumbstickYMin.localRotation, thumbstickNormalized.y);

                lastThumbstickPosition = newThumbstickPosition;
            }
        }

        public void AnimateTouchpad(bool isTouchpadPressed, bool isTouchpadTouched, Vector2 newTouchpadPosition)
        {
            if (touchpadPress != null && touchpadPressed != null && touchpadUnpressed != null && isTouchpadPressed != wasTouchpadPressed)
            {
                SetLocalPositionAndRotation(touchpadPress, isTouchpadPressed ? touchpadPressed : touchpadUnpressed);
                wasTouchpadPressed = isTouchpadPressed;
            }

            if (touchpadTouchVisualizer != null && isTouchpadTouched != wasTouchpadTouched)
            {
                touchpadTouchVisualizer.SetActive(isTouchpadTouched);
                wasTouchpadTouched = isTouchpadTouched;
            }

            if (touchpadTouchX != null && touchpadTouchY != null && touchpadTouchXMin != null && touchpadTouchXMax != null && touchpadTouchYMin != null && touchpadTouchYMax != null && newTouchpadPosition != lastTouchpadPosition)
            {
                Vector2 touchpadNormalized = (newTouchpadPosition + Vector2.one) * 0.5f;

                touchpadTouchX.transform.localPosition = Vector3.Lerp(touchpadTouchXMin.localPosition, touchpadTouchXMax.localPosition, touchpadNormalized.x);
                touchpadTouchX.transform.localRotation = Quaternion.Lerp(touchpadTouchXMin.localRotation, touchpadTouchXMax.localRotation, touchpadNormalized.x);

                touchpadTouchY.transform.localPosition = Vector3.Lerp(touchpadTouchYMax.localPosition, touchpadTouchYMin.localPosition, touchpadNormalized.y);
                touchpadTouchY.transform.localRotation = Quaternion.Lerp(touchpadTouchYMax.localRotation, touchpadTouchYMin.localRotation, touchpadNormalized.y);

                lastTouchpadPosition = newTouchpadPosition;
            }
        }

        private void SetLocalPositionAndRotation(GameObject buttonGameObject, Transform newTransform)
        {
            buttonGameObject.transform.localPosition = newTransform.localPosition;
            buttonGameObject.transform.localRotation = newTransform.localRotation;
        }

        public void SetRenderersVisible(bool visible)
        {
            MeshRenderer[] renderers = ControllerParent.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = visible;
            }
        }

        private void CreatePathToTransform(Transform transform)
        {
            Transform parentTransform = transform.parent;
            SDK_BaseController.ControllerElements controllerElement = SDK_BaseController.ControllerElements.AttachPoint;

            string path = transform.name;

            while (parentTransform != ControllerParent.transform)
            {
                switch (parentTransform.name.ToLower())
                {
                    case "home":
                        controllerElement = SDK_BaseController.ControllerElements.SystemMenu;
                        break;
                    case "menu":
                        controllerElement = SDK_BaseController.ControllerElements.StartMenu;
                        break;
                    case "grasp":
                        controllerElement = SDK_BaseController.ControllerElements.GripLeft;
                        break;
                    case "select":
                        controllerElement = SDK_BaseController.ControllerElements.Trigger;
                        break;
                    case "thumbstick_press":
                        controllerElement = SDK_BaseController.ControllerElements.TouchpadTwo;
                        break;
                    case "touchpad_press":
                        controllerElement = SDK_BaseController.ControllerElements.Touchpad;
                        break;
                }

                path = parentTransform.name + "/" + path;

                parentTransform = parentTransform.parent;
            }

            switch (controllerElement)
            {
                case SDK_BaseController.ControllerElements.SystemMenu:
                    homePath = path;
                    break;
                case SDK_BaseController.ControllerElements.StartMenu:
                    menuPath = path;
                    break;
                case SDK_BaseController.ControllerElements.GripLeft:
                    graspPath = path;
                    break;
                case SDK_BaseController.ControllerElements.Trigger:
                    selectPath = path;
                    break;
                case SDK_BaseController.ControllerElements.TouchpadTwo:
                    thumbstickPath = path;
                    break;
                case SDK_BaseController.ControllerElements.Touchpad:
                    touchpadPath = path;
                    break;
            }
        }

        public string GetPathToVisualizedButton(SDK_BaseController.ControllerElements button)
        {
            switch (button)
            {
                case SDK_BaseController.ControllerElements.SystemMenu:
                    return homePath;
                case SDK_BaseController.ControllerElements.StartMenu:
                    return menuPath;
                case SDK_BaseController.ControllerElements.GripLeft:
                case SDK_BaseController.ControllerElements.GripRight:
                    return graspPath;
                case SDK_BaseController.ControllerElements.Trigger:
                    return selectPath;
                case SDK_BaseController.ControllerElements.TouchpadTwo:
                    return thumbstickPath;
                case SDK_BaseController.ControllerElements.Touchpad:
                    return touchpadPath;
                default:
                    return null;
            }
        }
    }
}