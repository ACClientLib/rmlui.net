#include "RmlNative.h"
#include "RmlUi/Core/RenderInterface.h"
#include <iostream>
#include <string>

typedef Rml::CompiledGeometryHandle(*onCompileGeometry)(const Rml::Vertex *vertices,
                                int num_vertices,
                                const int *indices,
                                int num_indices);

typedef void(*onRenderGeometry)(Rml::CompiledGeometryHandle geometry,
                                Rml::Vector2f translation,
                                Rml::TextureHandle texture);

typedef void(*onReleaseGeometry)(Rml::CompiledGeometryHandle geometry);

typedef Rml::TextureHandle(*onLoadTexture)(Rml::Vector2i& texture_dimensions,
                                const Rml::String& source);

typedef Rml::TextureHandle(*onGenerateTexture)(const Rml::byte *source,
                                int num_bytes,
                                Rml::Vector2i source_dimensions);

typedef void(*onReleaseTexture)(Rml::TextureHandle texture);

typedef void(*onEnableScissorRegion)(bool enable);

typedef void(*onSetScissorRegion)(Rml::Rectanglei region);

class RenderInterface : Rml::RenderInterface {
private:
    ::onCompileGeometry m_onCompileGeometry;
    ::onRenderGeometry m_onRenderGeometry;
    ::onReleaseGeometry m_onReleaseGeometry;
    ::onLoadTexture m_onLoadTexture;
    ::onGenerateTexture m_onGenerateTexture;
    ::onReleaseTexture m_onReleaseTexture;
    ::onEnableScissorRegion m_onEnableScissorRegion;
    ::onSetScissorRegion m_onSetScissorRegion;

public:
    explicit RenderInterface(::onCompileGeometry onCompileGeometry,
                                ::onRenderGeometry onRenderGeometry,
                                ::onReleaseGeometry onReleaseGeometry,
                                ::onLoadTexture onLoadTexture,
                                ::onGenerateTexture onGenerateTexture,
                                ::onReleaseTexture onReleaseTexture,
                                ::onEnableScissorRegion onEnableScissorRegion,
                                ::onSetScissorRegion onSetScissorRegion) {
        m_onCompileGeometry = onCompileGeometry;
        m_onRenderGeometry = onRenderGeometry;
        m_onReleaseGeometry = onReleaseGeometry;
        m_onLoadTexture = onLoadTexture;
        m_onGenerateTexture = onGenerateTexture;
        m_onReleaseTexture = onReleaseTexture;
        m_onEnableScissorRegion = onEnableScissorRegion;
        m_onSetScissorRegion = onSetScissorRegion;
    }

    Rml::CompiledGeometryHandle CompileGeometry(Rml::Span<const Rml::Vertex> vertices, Rml::Span<const int> indices) override {
        return (*m_onCompileGeometry)(vertices.data(), vertices.size(), indices.data(), indices.size());
    }

    void RenderGeometry(Rml::CompiledGeometryHandle geometry, Rml::Vector2f translation, Rml::TextureHandle texture) override {
        (*m_onRenderGeometry)(geometry, translation, texture);
    }

    void ReleaseGeometry(Rml::CompiledGeometryHandle geometry) override {
        (*m_onReleaseGeometry)(geometry);
    }

    Rml::TextureHandle LoadTexture(Rml::Vector2i& texture_dimensions, const Rml::String& source) override {
        return (*m_onLoadTexture)(texture_dimensions, source);
    }

    Rml::TextureHandle GenerateTexture(Rml::Span<const Rml::byte> source, Rml::Vector2i source_dimensions) override {
        return (*m_onGenerateTexture)(source.data(), source.size(), source_dimensions);
    }

    void ReleaseTexture(Rml::TextureHandle texture) override {
        (*m_onReleaseTexture)(texture);
    }

    void EnableScissorRegion(bool enable) override {
        (*m_onEnableScissorRegion)(enable);
    }

    void SetScissorRegion(Rml::Rectanglei region) override {
        (*m_onSetScissorRegion)(region);
    }
};

RMLUI_CAPI void *rml_RenderInterface_New(::onCompileGeometry onCompileGeometry,
                                            ::onRenderGeometry onRenderGeometry,
                                            ::onReleaseGeometry onReleaseGeometry,
                                            ::onLoadTexture onLoadTexture,
                                            ::onGenerateTexture onGenerateTexture,
                                            ::onReleaseTexture onReleaseTexture,
                                            ::onEnableScissorRegion onEnableScissorRegion,
                                            ::onSetScissorRegion onSetScissorRegion) {
    return new RenderInterface(onCompileGeometry, onRenderGeometry, onReleaseGeometry, onLoadTexture, onGenerateTexture, onReleaseTexture, onEnableScissorRegion, onSetScissorRegion);
}