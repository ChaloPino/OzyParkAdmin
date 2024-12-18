﻿using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;
using MudBlazor;
using OzyParkAdmin.Shared;
using System.Globalization;
using System.Text;

namespace OzyParkAdmin.Components.Account.Shared;

/// <summary>
/// El layout para el tema estático.
/// </summary>
public partial class StaticThemeLayout : LayoutComponentBase
{
    private const string Palette = "mud-palette";
    private const string Ripple = "mud-ripple";
    private const string Elevation = "mud-elevation";
    private const string Typography = "mud-typography";
    private const string LayoutProperties = "mud";
    private const string Zindex = "mud-zindex";

    [Inject]
    public ThemeOzyPark Theme { get; set; } = default!;

    private string BuildTheme(bool isDarkMode)
    {
        MudTheme theme = Theme.ThemeManager.Theme;
        var builder = new StringBuilder();

        builder.Append(theme.PseudoCss.Scope);
        builder.Append('{');
        GenerateTheme(theme, builder, isDarkMode);
        builder.Append('}');
        return builder.ToString();
    }

    private void GenerateTheme(MudTheme theme, StringBuilder builder, bool isDarkMode)
    {
        Palette palette = isDarkMode ? theme.PaletteDark : theme.PaletteLight;
        //Palette
        builder.Append($"--{Palette}-black: {palette.Black};");
        builder.Append($"--{Palette}-white: {palette.White};");

        builder.Append($"--{Palette}-primary: {palette.Primary};");
        builder.Append(
            $"--{Palette}-primary-rgb: {palette.Primary.ToString(MudColorOutputFormats.ColorElements)};");
        builder.Append($"--{Palette}-primary-text: {palette.PrimaryContrastText};");
        builder.Append($"--{Palette}-primary-darken: {palette.PrimaryDarken};");
        builder.Append($"--{Palette}-primary-lighten: {palette.PrimaryLighten};");
        builder.Append(
            $"--{Palette}-primary-hover: {palette.Primary.SetAlpha(palette.HoverOpacity).ToString(MudColorOutputFormats.RGBA)};");
        builder.Append($"--{Palette}-secondary: {palette.Secondary};");
        builder.Append(
            $"--{Palette}-secondary-rgb: {palette.Secondary.ToString(MudColorOutputFormats.ColorElements)};");
        builder.Append($"--{Palette}-secondary-text: {palette.SecondaryContrastText};");
        builder.Append($"--{Palette}-secondary-darken: {palette.SecondaryDarken};");
        builder.Append($"--{Palette}-secondary-lighten: {palette.SecondaryLighten};");
        builder.Append(
            $"--{Palette}-secondary-hover: {palette.Secondary.SetAlpha(palette.HoverOpacity).ToString(MudColorOutputFormats.RGBA)};");
        builder.Append($"--{Palette}-tertiary: {palette.Tertiary};");
        builder.Append(
            $"--{Palette}-tertiary-rgb: {palette.Tertiary.ToString(MudColorOutputFormats.ColorElements)};");
        builder.Append($"--{Palette}-tertiary-text: {palette.TertiaryContrastText};");
        builder.Append($"--{Palette}-tertiary-darken: {palette.TertiaryDarken};");
        builder.Append($"--{Palette}-tertiary-lighten: {palette.TertiaryLighten};");
        builder.Append(
            $"--{Palette}-tertiary-hover: {palette.Tertiary.SetAlpha(palette.HoverOpacity).ToString(MudColorOutputFormats.RGBA)};");
        builder.Append($"--{Palette}-info: {palette.Info};");
        builder.Append(
            $"--{Palette}-info-rgb: {palette.Info.ToString(MudColorOutputFormats.ColorElements)};");
        builder.Append($"--{Palette}-info-text: {palette.InfoContrastText};");
        builder.Append($"--{Palette}-info-darken: {palette.InfoDarken};");
        builder.Append($"--{Palette}-info-lighten: {palette.InfoLighten};");
        builder.Append(
            $"--{Palette}-info-hover: {palette.Info.SetAlpha(palette.HoverOpacity).ToString(MudColorOutputFormats.RGBA)};");
        builder.Append($"--{Palette}-success: {palette.Success};");
        builder.Append(
            $"--{Palette}-success-rgb: {palette.Success.ToString(MudColorOutputFormats.ColorElements)};");
        builder.Append($"--{Palette}-success-text: {palette.SuccessContrastText};");
        builder.Append($"--{Palette}-success-darken: {palette.SuccessDarken};");
        builder.Append($"--{Palette}-success-lighten: {palette.SuccessLighten};");
        builder.Append(
            $"--{Palette}-success-hover: {palette.Success.SetAlpha(palette.HoverOpacity).ToString(MudColorOutputFormats.RGBA)};");
        builder.Append($"--{Palette}-warning: {palette.Warning};");
        builder.Append(
            $"--{Palette}-warning-rgb: {palette.Warning.ToString(MudColorOutputFormats.ColorElements)};");
        builder.Append($"--{Palette}-warning-text: {palette.WarningContrastText};");
        builder.Append($"--{Palette}-warning-darken: {palette.WarningDarken};");
        builder.Append($"--{Palette}-warning-lighten: {palette.WarningLighten};");
        builder.Append(
            $"--{Palette}-warning-hover: {palette.Warning.SetAlpha(palette.HoverOpacity).ToString(MudColorOutputFormats.RGBA)};");
        builder.Append($"--{Palette}-error: {palette.Error};");
        builder.Append(
            $"--{Palette}-error-rgb: {palette.Error.ToString(MudColorOutputFormats.ColorElements)};");
        builder.Append($"--{Palette}-error-text: {palette.ErrorContrastText};");
        builder.Append($"--{Palette}-error-darken: {palette.ErrorDarken};");
        builder.Append($"--{Palette}-error-lighten: {palette.ErrorLighten};");
        builder.Append(
            $"--{Palette}-error-hover: {palette.Error.SetAlpha(palette.HoverOpacity).ToString(MudColorOutputFormats.RGBA)};");
        builder.Append($"--{Palette}-dark: {palette.Dark};");
        builder.Append(
            $"--{Palette}-dark-rgb: {palette.Dark.ToString(MudColorOutputFormats.ColorElements)};");
        builder.Append($"--{Palette}-dark-text: {palette.DarkContrastText};");
        builder.Append($"--{Palette}-dark-darken: {palette.DarkDarken};");
        builder.Append($"--{Palette}-dark-lighten: {palette.DarkLighten};");
        builder.Append(
            $"--{Palette}-dark-hover: {palette.Dark.SetAlpha(palette.HoverOpacity).ToString(MudColorOutputFormats.RGBA)};");

        builder.Append($"--{Palette}-text-primary: {palette.TextPrimary};");
        builder.Append($"--{Palette}-text-secondary: {palette.TextSecondary};");
        builder.Append($"--{Palette}-text-disabled: {palette.TextDisabled};");

        builder.Append($"--{Palette}-action-default: {palette.ActionDefault};");
        builder.Append(
            $"--{Palette}-action-default-hover: {palette.ActionDefault.SetAlpha(palette.HoverOpacity).ToString(MudColorOutputFormats.RGBA)};");
        builder.Append($"--{Palette}-action-disabled: {palette.ActionDisabled};");
        builder.Append(
            $"--{Palette}-action-disabled-background: {palette.ActionDisabledBackground};");

        builder.Append($"--{Palette}-surface: {palette.Surface};");
        builder.Append($"--{Palette}-background: {palette.Background};");
        builder.Append($"--{Palette}-background-gray: {palette.BackgroundGray};");
        builder.Append($"--{Palette}-drawer-background: {palette.DrawerBackground};");
        builder.Append($"--{Palette}-drawer-text: {palette.DrawerText};");
        builder.Append($"--{Palette}-drawer-icon: {palette.DrawerIcon};");
        builder.Append($"--{Palette}-appbar-background: {palette.AppbarBackground};");
        builder.Append($"--{Palette}-appbar-text: {palette.AppbarText};");

        builder.Append($"--{Palette}-lines-default: {palette.LinesDefault};");
        builder.Append($"--{Palette}-lines-inputs: {palette.LinesInputs};");

        builder.Append($"--{Palette}-table-lines: {palette.TableLines};");
        builder.Append($"--{Palette}-table-striped: {palette.TableStriped};");
        builder.Append($"--{Palette}-table-hover: {palette.TableHover};");

        builder.Append($"--{Palette}-divider: {palette.Divider};");
        builder.Append($"--{Palette}-divider-light: {palette.DividerLight};");

        builder.Append($"--{Palette}-gray-default: {palette.GrayDefault};");
        builder.Append($"--{Palette}-gray-light: {palette.GrayLight};");
        builder.Append($"--{Palette}-gray-lighter: {palette.GrayLighter};");
        builder.Append($"--{Palette}-gray-dark: {palette.GrayDark};");
        builder.Append($"--{Palette}-gray-darker: {palette.GrayDarker};");

        builder.Append($"--{Palette}-overlay-dark: {palette.OverlayDark};");
        builder.Append($"--{Palette}-overlay-light: {palette.OverlayLight};");

        //Ripple
        builder.Append($"--{Ripple}-color: var(--{Palette}-text-primary);");
        builder.Append($"--{Ripple}-opacity: {theme.PaletteLight.RippleOpacity.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Ripple}-opacity-secondary: {theme.PaletteLight.RippleOpacitySecondary.ToString(CultureInfo.InvariantCulture)};");

        //Elevations
        builder.Append($"--{Elevation}-0: {theme.Shadows.Elevation.GetValue(0)};");
        builder.Append($"--{Elevation}-1: {theme.Shadows.Elevation.GetValue(1)};");
        builder.Append($"--{Elevation}-2: {theme.Shadows.Elevation.GetValue(2)};");
        builder.Append($"--{Elevation}-3: {theme.Shadows.Elevation.GetValue(3)};");
        builder.Append($"--{Elevation}-4: {theme.Shadows.Elevation.GetValue(4)};");
        builder.Append($"--{Elevation}-5: {theme.Shadows.Elevation.GetValue(5)};");
        builder.Append($"--{Elevation}-6: {theme.Shadows.Elevation.GetValue(6)};");
        builder.Append($"--{Elevation}-7: {theme.Shadows.Elevation.GetValue(7)};");
        builder.Append($"--{Elevation}-8: {theme.Shadows.Elevation.GetValue(8)};");
        builder.Append($"--{Elevation}-9: {theme.Shadows.Elevation.GetValue(9)};");
        builder.Append($"--{Elevation}-10: {theme.Shadows.Elevation.GetValue(10)};");
        builder.Append($"--{Elevation}-11: {theme.Shadows.Elevation.GetValue(11)};");
        builder.Append($"--{Elevation}-12: {theme.Shadows.Elevation.GetValue(12)};");
        builder.Append($"--{Elevation}-13: {theme.Shadows.Elevation.GetValue(13)};");
        builder.Append($"--{Elevation}-14: {theme.Shadows.Elevation.GetValue(14)};");
        builder.Append($"--{Elevation}-15: {theme.Shadows.Elevation.GetValue(15)};");
        builder.Append($"--{Elevation}-16: {theme.Shadows.Elevation.GetValue(16)};");
        builder.Append($"--{Elevation}-17: {theme.Shadows.Elevation.GetValue(17)};");
        builder.Append($"--{Elevation}-18: {theme.Shadows.Elevation.GetValue(18)};");
        builder.Append($"--{Elevation}-19: {theme.Shadows.Elevation.GetValue(19)};");
        builder.Append($"--{Elevation}-20: {theme.Shadows.Elevation.GetValue(20)};");
        builder.Append($"--{Elevation}-21: {theme.Shadows.Elevation.GetValue(21)};");
        builder.Append($"--{Elevation}-22: {theme.Shadows.Elevation.GetValue(22)};");
        builder.Append($"--{Elevation}-23: {theme.Shadows.Elevation.GetValue(23)};");
        builder.Append($"--{Elevation}-24: {theme.Shadows.Elevation.GetValue(24)};");
        builder.Append($"--{Elevation}-25: {theme.Shadows.Elevation.GetValue(25)};");

        //Layout Properties
        builder.Append(
            $"--{LayoutProperties}-default-borderradius: {theme.LayoutProperties.DefaultBorderRadius};");
        builder.Append($"--{LayoutProperties}-drawer-width-left: {theme.LayoutProperties.DrawerWidthLeft};");
        builder.Append($"--{LayoutProperties}-drawer-width-right: {theme.LayoutProperties.DrawerWidthRight};");
        builder.Append(
            $"--{LayoutProperties}-drawer-width-mini-left: {theme.LayoutProperties.DrawerMiniWidthLeft};");
        builder.Append(
            $"--{LayoutProperties}-drawer-width-mini-right: {theme.LayoutProperties.DrawerMiniWidthRight};");
        builder.Append($"--{LayoutProperties}-appbar-height: {theme.LayoutProperties.AppbarHeight};");

        //Breakpoint
        //theme.Append($"--{Breakpoint}-xs: {Theme.Breakpoints.xs};");
        //theme.Append($"--{Breakpoint}-sm: {Theme.Breakpoints.sm};");
        //theme.Append($"--{Breakpoint}-md: {Theme.Breakpoints.md};");
        //theme.Append($"--{Breakpoint}-lg: {Theme.Breakpoints.lg};");
        //theme.Append($"--{Breakpoint}-xl: {Theme.Breakpoints.xl};");
        //theme.Append($"--{Breakpoint}-xxl: {Theme.Breakpoints.xxl};");

        //Typography
        builder.Append(
            $"--{Typography}-default-family: '{string.Join("','", theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-default-size: {theme.Typography.Default.FontSize};");
        builder.Append($"--{Typography}-default-weight: {theme.Typography.Default.FontWeight};");
        builder.Append(
            $"--{Typography}-default-lineheight: {theme.Typography.Default.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-default-letterspacing: {theme.Typography.Default.LetterSpacing};");
        builder.Append($"--{Typography}-default-text-transform: {theme.Typography.Default.TextTransform};");

        builder.Append(
            $"--{Typography}-h1-family: '{string.Join("','", theme.Typography.H1.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-h1-size: {theme.Typography.H1.FontSize};");
        builder.Append($"--{Typography}-h1-weight: {theme.Typography.H1.FontWeight};");
        builder.Append(
            $"--{Typography}-h1-lineheight: {theme.Typography.H1.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-h1-letterspacing: {theme.Typography.H1.LetterSpacing};");
        builder.Append($"--{Typography}-h1-text-transform: {theme.Typography.H1.TextTransform};");

        builder.Append(
            $"--{Typography}-h2-family: '{string.Join("','", theme.Typography.H2.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-h2-size: {theme.Typography.H2.FontSize};");
        builder.Append($"--{Typography}-h2-weight: {theme.Typography.H2.FontWeight};");
        builder.Append(
            $"--{Typography}-h2-lineheight: {theme.Typography.H2.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-h2-letterspacing: {theme.Typography.H2.LetterSpacing};");
        builder.Append($"--{Typography}-h2-text-transform: {theme.Typography.H2.TextTransform};");

        builder.Append(
            $"--{Typography}-h3-family: '{string.Join("','", theme.Typography.H3.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-h3-size: {theme.Typography.H3.FontSize};");
        builder.Append($"--{Typography}-h3-weight: {theme.Typography.H3.FontWeight};");
        builder.Append(
            $"--{Typography}-h3-lineheight: {theme.Typography.H3.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-h3-letterspacing: {theme.Typography.H3.LetterSpacing};");
        builder.Append($"--{Typography}-h3-text-transform: {theme.Typography.H3.TextTransform};");

        builder.Append(
            $"--{Typography}-h4-family: '{string.Join("','", theme.Typography.H4.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-h4-size: {theme.Typography.H4.FontSize};");
        builder.Append($"--{Typography}-h4-weight: {theme.Typography.H4.FontWeight};");
        builder.Append(
            $"--{Typography}-h4-lineheight: {theme.Typography.H4.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-h4-letterspacing: {theme.Typography.H4.LetterSpacing};");
        builder.Append($"--{Typography}-h4-text-transform: {theme.Typography.H4.TextTransform};");

        builder.Append(
            $"--{Typography}-h5-family: '{string.Join("','", theme.Typography.H5.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-h5-size: {theme.Typography.H5.FontSize};");
        builder.Append($"--{Typography}-h5-weight: {theme.Typography.H5.FontWeight};");
        builder.Append(
            $"--{Typography}-h5-lineheight: {theme.Typography.H5.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-h5-letterspacing: {theme.Typography.H5.LetterSpacing};");
        builder.Append($"--{Typography}-h5-text-transform: {theme.Typography.H5.TextTransform};");

        builder.Append(
            $"--{Typography}-h6-family: '{string.Join("','", theme.Typography.H6.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-h6-size: {theme.Typography.H6.FontSize};");
        builder.Append($"--{Typography}-h6-weight: {theme.Typography.H6.FontWeight};");
        builder.Append(
            $"--{Typography}-h6-lineheight: {theme.Typography.H6.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-h6-letterspacing: {theme.Typography.H6.LetterSpacing};");
        builder.Append($"--{Typography}-h6-text-transform: {theme.Typography.H6.TextTransform};");

        builder.Append(
            $"--{Typography}-subtitle1-family: '{string.Join("','", theme.Typography.Subtitle1.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-subtitle1-size: {theme.Typography.Subtitle1.FontSize};");
        builder.Append($"--{Typography}-subtitle1-weight: {theme.Typography.Subtitle1.FontWeight};");
        builder.Append(
            $"--{Typography}-subtitle1-lineheight: {theme.Typography.Subtitle1.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-subtitle1-letterspacing: {theme.Typography.Subtitle1.LetterSpacing};");
        builder.Append($"--{Typography}-subtitle1-text-transform: {theme.Typography.Subtitle1.TextTransform};");

        builder.Append(
            $"--{Typography}-subtitle2-family: '{string.Join("','", theme.Typography.Subtitle2.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-subtitle2-size: {theme.Typography.Subtitle2.FontSize};");
        builder.Append($"--{Typography}-subtitle2-weight: {theme.Typography.Subtitle2.FontWeight};");
        builder.Append(
            $"--{Typography}-subtitle2-lineheight: {theme.Typography.Subtitle2.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-subtitle2-letterspacing: {theme.Typography.Subtitle2.LetterSpacing};");
        builder.Append($"--{Typography}-subtitle2-text-transform: {theme.Typography.Subtitle2.TextTransform};");

        builder.Append(
            $"--{Typography}-body1-family: '{string.Join("','", theme.Typography.Body1.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-body1-size: {theme.Typography.Body1.FontSize};");
        builder.Append($"--{Typography}-body1-weight: {theme.Typography.Body1.FontWeight};");
        builder.Append(
            $"--{Typography}-body1-lineheight: {theme.Typography.Body1.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-body1-letterspacing: {theme.Typography.Body1.LetterSpacing};");
        builder.Append($"--{Typography}-body1-text-transform: {theme.Typography.Body1.TextTransform};");

        builder.Append(
            $"--{Typography}-body2-family: '{string.Join("','", theme.Typography.Body2.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-body2-size: {theme.Typography.Body2.FontSize};");
        builder.Append($"--{Typography}-body2-weight: {theme.Typography.Body2.FontWeight};");
        builder.Append(
            $"--{Typography}-body2-lineheight: {theme.Typography.Body2.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-body2-letterspacing: {theme.Typography.Body2.LetterSpacing};");
        builder.Append($"--{Typography}-body2-text-transform: {theme.Typography.Body2.TextTransform};");

        builder.Append(
            $"--{Typography}-input-family: '{string.Join("','", theme.Typography.Input.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-input-size: {theme.Typography.Input.FontSize};");
        builder.Append($"--{Typography}-input-weight: {theme.Typography.Input.FontWeight};");
        builder.Append(
            $"--{Typography}-input-lineheight: {theme.Typography.Input.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-input-letterspacing: {theme.Typography.Input.LetterSpacing};");
        builder.Append($"--{Typography}-input-text-transform: {theme.Typography.Input.TextTransform};");

        builder.Append(
            $"--{Typography}-button-family: '{string.Join("','", theme.Typography.Button.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-button-size: {theme.Typography.Button.FontSize};");
        builder.Append($"--{Typography}-button-weight: {theme.Typography.Button.FontWeight};");
        builder.Append(
            $"--{Typography}-button-lineheight: {theme.Typography.Button.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-button-letterspacing: {theme.Typography.Button.LetterSpacing};");
        builder.Append($"--{Typography}-button-text-transform: {theme.Typography.Button.TextTransform};");

        builder.Append(
            $"--{Typography}-caption-family: '{string.Join("','", theme.Typography.Caption.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-caption-size: {theme.Typography.Caption.FontSize};");
        builder.Append($"--{Typography}-caption-weight: {theme.Typography.Caption.FontWeight};");
        builder.Append(
            $"--{Typography}-caption-lineheight: {theme.Typography.Caption.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-caption-letterspacing: {theme.Typography.Caption.LetterSpacing};");
        builder.Append($"--{Typography}-caption-text-transform: {theme.Typography.Caption.TextTransform};");

        builder.Append(
            $"--{Typography}-overline-family: '{string.Join("','", theme.Typography.Overline.FontFamily ?? theme.Typography.Default.FontFamily ?? Array.Empty<string>())}';");
        builder.Append($"--{Typography}-overline-size: {theme.Typography.Overline.FontSize};");
        builder.Append($"--{Typography}-overline-weight: {theme.Typography.Overline.FontWeight};");
        builder.Append(
            $"--{Typography}-overline-lineheight: {theme.Typography.Overline.LineHeight.ToString(CultureInfo.InvariantCulture)};");
        builder.Append($"--{Typography}-overline-letterspacing: {theme.Typography.Overline.LetterSpacing};");
        builder.Append($"--{Typography}-overline-text-transform: {theme.Typography.Overline.TextTransform};");

        //Z-Index
        builder.Append($"--{Zindex}-drawer: {theme.ZIndex.Drawer};");
        builder.Append($"--{Zindex}-appbar: {theme.ZIndex.AppBar};");
        builder.Append($"--{Zindex}-dialog: {theme.ZIndex.Dialog};");
        builder.Append($"--{Zindex}-popover: {theme.ZIndex.Popover};");
        builder.Append($"--{Zindex}-snackbar: {theme.ZIndex.Snackbar};");
        builder.Append($"--{Zindex}-tooltip: {theme.ZIndex.Tooltip};");

        // Native HTML control light/dark mode
        builder.Append($"--mud-native-html-color-scheme: {(isDarkMode ? "dark" : "light")};");
    }
}
