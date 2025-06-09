from PySide6.QtCore import Qt
from PySide6.QtWidgets import QWidget, QLabel, QVBoxLayout


class Settings(QWidget):
    def __init__(self, parent=None):
        super().__init__(parent)

        self.text = QLabel("Hello Settings!", self, alignment=Qt.AlignmentFlag.AlignCenter)

        self.layout = QVBoxLayout(self)
        self.layout.addWidget(self.text)