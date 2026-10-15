document.addEventListener('DOMContentLoaded', () => {

  const editInput = document.getElementById('editInput');
  const deleteBtn = document.getElementById('deleteBtn');

  if (!editInput || !deleteBtn) return;

  const initialValue = editInput.value;
  let changed = false;

  editInput.addEventListener('input', () => {

    if (!changed && editInput.value !== initialValue) {
      changed = true;
      deleteBtn.disabled = true;
    }
    if (editInput.value === initialValue) {
      changed = false;
      deleteBtn.disabled = false;
    }
  });
});