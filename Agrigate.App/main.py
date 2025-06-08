import sys

from PySide6 import QtWidgets
from PySide6.QtGui import QAction
from PySide6.QtWidgets import QMainWindow, QMenu

from components.main_menu.about import About

class Agrigate(QMainWindow):
    def __init__(self):
        super().__init__()

        self.setWindowTitle("Agrigate")
        self.create_file_menu()
        self.create_help_menu()

    def create_file_menu(self):
        file_menu = self.menuBar().addMenu("File")

        exit_action = QAction("Exit", self)
        exit_action.setShortcut("Ctrl+Q")
        exit_action.setStatusTip("Exit Agrigate")
        exit_action.triggered.connect(self.close)
        file_menu.addAction(exit_action)

    def create_help_menu(self):
        help_menu = self.menuBar().addMenu("Help")

        about_action = QAction("About", self)
        about_action.setShortcut("Ctrl+A")
        about_action.setStatusTip("About Agrigate")
        about_action.triggered.connect(self.show_about)
        help_menu.addAction(about_action)

    def show_about(self):
        about_dialog = About(self)
        about_dialog.exec()

if __name__ == "__main__":
    app = QtWidgets.QApplication([])

    window = Agrigate()
    window.resize(800, 600)
    window.show()

    sys.exit(app.exec())