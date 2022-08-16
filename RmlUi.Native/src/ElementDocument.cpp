
#include "RmlUi/Core/ElementDocument.h"

extern "C" void rml_ElementDocument_Show(Rml::ElementDocument* document, Rml::ModalFlag modal_flag = Rml::ModalFlag::None, Rml::FocusFlag focus_flag = Rml::FocusFlag::Auto) {
    document->Show(modal_flag, focus_flag);
}

extern "C" void rml_ElementDocument_Hide(Rml::ElementDocument* document) {
    document->Hide();
}

extern "C" void rml_ElementDocument_Close(Rml::ElementDocument* document) {
    document->Close();
}