using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace RmlUiNet
{
    public class Context : RmlBase<Context>
    {
        #region Properties

        /// <summary>
        /// Returns the name of the context.
        /// </summary>
        public string Name => Marshal.PtrToStringAnsi(Native.Context.GetName(NativePtr));

        /// <summary>
        /// Returns a hint on whether the mouse is currently interacting with any elements in this context.
        /// </summary>
        public bool IsMouseInteracting => Native.Context.IsMouseInteracting(NativePtr);

        #endregion

        #region Methods

        internal Context(IntPtr ptr) : base(ptr)
        {
        }

        public void Update()
        {
            Native.Context.Update(NativePtr);
        }

        public void Render()
        {
            Native.Context.Render(NativePtr);
        }

        public ElementDocument? LoadDocument(string documentPath)
        {
            return ElementDocument.Create(Native.Context.LoadDocument(NativePtr, documentPath));
        }

        public ElementDocument? LoadDocumentFromMemory(string documentRml, string sourceUrl = "[document from memory]")
        {
            return ElementDocument.Create(Native.Context.LoadDocumentFromMemory(NativePtr, documentRml, sourceUrl));
        }

        /// <summary>
        /// Tells the context the mouse has left the window. This removes any hover state from all elements and prevents
        /// 'Update()' from setting the hover state for elements under the mouse.
        /// </summary>
        /// <returns>True if the mouse is not interacting with any elements in the context (see <see cref="IsMouseInteracting"/>),
        /// otherwise false.</returns>
        public bool ProcessMouseLeave()
        {
            return Native.Context.ProcessMouseLeave(NativePtr);
        }

        /// <summary>
        /// Sends a mouse movement event into this context.
        /// </summary>
        /// <param name="x">The x-coordinate of the mouse cursor, in window-coordinates (ie, 0 should be the left of the client area).</param>
        /// <param name="y">The y-coordinate of the mouse cursor, in window-coordinates (ie, 0 should be the top of the client area).</param>
        /// <param name="keyModifierState">The state of key modifiers (shift, control, caps-lock, etc) keys; this should be generated by ORing together members of the Input::KeyModifier enumeration.</param>
        /// <returns>True if the mouse is not interacting with any elements in the context (see <see cref="IsMouseInteracting"/>), otherwise false.</returns>
        public bool ProcessMouseMove(int x, int y, int keyModifierState)
        {
            return Native.Context.ProcessMouseMove(NativePtr, x, y, keyModifierState);
        }

        /// <summary>
        /// Sends a mouse-button down event into this context.
        /// </summary>
        /// <param name="buttonIndex">The index of the button that was pressed; 0 for the left button, 1 for right, and any others from 2 onwards.</param>
        /// <param name="keyModifierState">The state of key modifiers (shift, control, caps-lock, etc) keys; this should be generated by ORing together members of the Input::KeyModifier enumeration.</param>
        /// <returns>True if the mouse is not interacting with any elements in the context (see <see cref="IsMouseInteracting"/>), otherwise false.</returns>
        public bool ProcessMouseButtonDown(int buttonIndex, int keyModifierState)
        {
            return Native.Context.ProcessMouseButtonDown(NativePtr, buttonIndex, keyModifierState);
        }

        /// <summary>
        /// Sends a mouse-button up event into this context.
        /// </summary>
        /// <param name="buttonIndex">The index of the button that was pressed; 0 for the left button, 1 for right, and any others from 2 onwards.</param>
        /// <param name="keyModifierState">The state of key modifiers (shift, control, caps-lock, etc) keys; this should be generated by ORing together members of the Input::KeyModifier enumeration.</param>
        /// <returns>True if the mouse is not interacting with any elements in the context (see <see cref="IsMouseInteracting"/>), otherwise false.</returns>
        public bool ProcessMouseButtonUp(int buttonIndex, int keyModifierState)
        {
            return Native.Context.ProcessMouseButtonUp(NativePtr, buttonIndex, keyModifierState);
        }

        /// <summary>
        /// Adds an event listener to the context's root element.
        /// </summary>
        /// <param name="name">The name of the event to attach to.</param>
        /// <param name="eventListener">Listener object to be attached.</param>
        /// <param name="inCapturePhase">True if the listener is to be attached to the capture phase, false for the bubble phase.</param>
        public void AddEventListener(string name, EventListener eventListener, bool inCapturePhase = false)
        {
            Native.Context.AddEventListener(NativePtr, name, eventListener.NativePtr, inCapturePhase);
        }

        /// <summary>
        /// Adds an event listener to the context's root element.
        /// </summary>
        /// <param name="name">The name of the event to attach to.</param>
        /// <param name="eventListener">Listener object to be attached.</param>
        /// <param name="inCapturePhase">True if the listener is to be attached to the capture phase, false for the bubble phase.</param>
        public void RemoveEventListener(string name, EventListener eventListener, bool inCapturePhase = false)
        {
            Native.Context.RemoveEventListener(NativePtr, name, eventListener.NativePtr, inCapturePhase);
        }

        internal static Context? Create(string name, Vector2i dimensions, RenderInterface? renderInterface = null)
        {
            return GetOrCreateCache(Native.Context.Create(name, dimensions, renderInterface?.NativePtr ?? IntPtr.Zero), ptr => new Context(ptr));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Native.Context.Delete(NativePtr);
        }

        #endregion
    }
}
