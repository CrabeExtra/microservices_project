import { themeQuartz } from "ag-grid-community";

// TODO: this is kind of messy, I should redo this as CSS.

export const terminalTheme = themeQuartz.withParams({
    backgroundColor: "#000000",
    foregroundColor: "#00FF41",
    headerBackgroundColor: "#222222",
    headerTextColor: "#F2F0EA",
    borderColor: "#333333",
    accentColor: "#00FF41",
    fontFamily: "VT323",
    fontSize: 18,
    spacing: 6,
    rowHoverColor: "#111111",
});