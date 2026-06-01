import { useState } from "react";
import {
  saveQuestion,
  editQuestion,
  deleteQuestion,
} from "../services/questionService";
import {
  saveAnswer,
  editAnswer,
  deleteAnswer,
} from "../services/answerService";

export default function QuestionsAnswersSection({
  qaComments,
  courseId,
  lectureId,
  onQuestionSaved,
  currentUserId,
}) {
  const [qaTitle, setQaTitle] = useState("");
  const [qaInput, setQaInput] = useState("");
  const [isAddingQuestion, setIsAddingQuestion] = useState(false);
  const [answeringQuestionId, setAnsweringQuestionId] = useState(null);
  const [expandedQuestions, setExpandedQuestions] = useState({});
  const [isSaving, setIsSaving] = useState(false);
  const [saveError, setSaveError] = useState(null);
  const [editingQuestionId, setEditingQuestionId] = useState(null);
  const [editingQuestionTitle, setEditingQuestionTitle] = useState("");
  const [editingQuestionContent, setEditingQuestionContent] = useState("");
  const [editingAnswerId, setEditingAnswerId] = useState(null);
  const [editingAnswerTitle, setEditingAnswerTitle] = useState("");
  const [editingAnswerContent, setEditingAnswerContent] = useState("");

  const handleSubmitQuestion = async (e) => {
    e.preventDefault();
    if (!qaTitle.trim() || !qaInput.trim()) {
      setSaveError("Both title and content are required");
      return;
    } else if (qaTitle.length < 3 || qaInput.length < 5) {
      setSaveError("Title or content is too short");
      return;
    }

    try {
      setIsSaving(true);
      setSaveError(null);
      await saveQuestion(courseId, lectureId, qaTitle, qaInput);

      // Reset form
      setQaTitle("");
      setQaInput("");
      setIsAddingQuestion(false);

      // Call the parent callback to refresh questions
      if (onQuestionSaved) {
        await onQuestionSaved();
      }
    } catch (error) {
      console.error("Failed to save question:", error);
      setSaveError(error.message || "Failed to save question");
    } finally {
      setIsSaving(false);
    }
  };

  const handleSubmitAnswer = async (e, questionId) => {
    e.preventDefault();
    if (!qaInput.trim() || !qaTitle.trim()) {
      setSaveError("Answer content cannot be empty");
      return;
    } else if (qaTitle.length < 3 || qaInput.length < 5) {
      setSaveError("Title or content is too short");
      return;
    }

    try {
      setIsSaving(true);
      setSaveError(null);
      await saveAnswer(questionId, qaTitle, qaInput);

      // Reset form
      setQaInput("");
      setQaTitle("");
      setAnsweringQuestionId(null);

      // Call the parent callback to refresh questions (and their answers)
      if (onQuestionSaved) {
        await onQuestionSaved();
      }
    } catch (error) {
      console.error("Failed to save answer:", error);
      setSaveError(error.message || "Failed to save answer");
    } finally {
      setIsSaving(false);
    }
  };

  const toggleQuestion = (questionId) => {
    setExpandedQuestions((prev) => ({
      ...prev,
      [questionId]: !prev[questionId],
    }));
  };

  const handleStartEditQuestion = (question) => {
    setEditingQuestionId(question.id);
    setEditingQuestionTitle(question.title || "");
    setEditingQuestionContent(question.content || "");
    setSaveError(null);
    setIsAddingQuestion(false);
    setAnsweringQuestionId(null);
  };

  const handleCancelEditQuestion = () => {
    setEditingQuestionId(null);
    setEditingQuestionTitle("");
    setEditingQuestionContent("");
    setSaveError(null);
  };

  const handleSaveEditQuestion = async (questionId) => {
    if (!editingQuestionTitle.trim() || !editingQuestionContent.trim()) {
      setSaveError("Title and content are required");
      return;
    }

    try {
      setIsSaving(true);
      setSaveError(null);
      await editQuestion(
        questionId,
        editingQuestionTitle,
        editingQuestionContent,
      );
      setEditingQuestionId(null);
      setEditingQuestionTitle("");
      setEditingQuestionContent("");
      if (onQuestionSaved) await onQuestionSaved();
    } catch (error) {
      console.error("Failed to edit question:", error);
      setSaveError(error.message || "Failed to edit question");
    } finally {
      setIsSaving(false);
    }
  };

  const handleDeleteQuestion = async (questionId) => {
    if (!questionId) return;
    try {
      setIsSaving(true);
      setSaveError(null);
      await deleteQuestion(questionId);
      if (onQuestionSaved) await onQuestionSaved();
    } catch (error) {
      console.error("Failed to delete question:", error);
      setSaveError(error.message || "Failed to delete question");
    } finally {
      setIsSaving(false);
    }
  };

  const handleAnswerClick = (questionId) => {
    setAnsweringQuestionId(questionId);
    setIsAddingQuestion(false);
    setQaTitle("");
    setQaInput("");
    setExpandedQuestions((prev) => ({ ...prev, [questionId]: true }));
  };

  const handleAddQuestionClick = () => {
    setIsAddingQuestion(true);
    setAnsweringQuestionId(null);
    setQaTitle("");
    setQaInput("");
  };

  const handleCancelForm = () => {
    setIsAddingQuestion(false);
    setAnsweringQuestionId(null);
    setQaTitle("");
    setQaInput("");
    setSaveError(null);
  };

  const handleStartEditAnswer = (answer) => {
    setEditingAnswerId(answer.id);
    setEditingAnswerTitle(answer.title || "");
    setEditingAnswerContent(answer.content || "");
    setSaveError(null);
  };

  const handleCancelEditAnswer = () => {
    setEditingAnswerId(null);
    setEditingAnswerTitle("");
    setEditingAnswerContent("");
    setSaveError(null);
  };

  const handleSaveEditAnswer = async (answerId) => {
    if (!editingAnswerTitle.trim() || !editingAnswerContent.trim()) {
      setSaveError("Title and content are required");
      return;
    }

    try {
      setIsSaving(true);
      setSaveError(null);
      await editAnswer(answerId, editingAnswerTitle, editingAnswerContent);
      setEditingAnswerId(null);
      setEditingAnswerTitle("");
      setEditingAnswerContent("");
      if (onQuestionSaved) await onQuestionSaved();
    } catch (error) {
      console.error("Failed to edit answer:", error);
      setSaveError(error.message || "Failed to edit answer");
    } finally {
      setIsSaving(false);
    }
  };

  const handleDeleteAnswer = async (answerId) => {
    if (!answerId) return;
    try {
      setIsSaving(true);
      setSaveError(null);
      await deleteAnswer(answerId);
      if (onQuestionSaved) await onQuestionSaved();
    } catch (error) {
      console.error("Failed to delete answer:", error);
      setSaveError(error.message || "Failed to delete answer");
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="qa-section">
      <div className="qa-header">
        <h3>Public Q/A (for this lecture)</h3>
        <button
          className="add-question-btn"
          onClick={handleAddQuestionClick}
          disabled={isAddingQuestion}
        >
          + Ask Question
        </button>
      </div>

      {isAddingQuestion && (
        <form
          className="qa-form new-question-form"
          onSubmit={handleSubmitQuestion}
        >
          {saveError && (
            <div
              className="qa-error"
              style={{
                color: "#dc2626",
                fontSize: "14px",
                marginBottom: "8px",
              }}
            >
              {saveError}
            </div>
          )}
          <input
            type="text"
            className="qa-title-input"
            value={qaTitle}
            onChange={(e) => {
              setQaTitle(e.target.value);
              if (saveError) setSaveError(null);
            }}
            placeholder="Question title"
            autoFocus
            disabled={isSaving}
          />
          <textarea
            value={qaInput}
            onChange={(e) => {
              setQaInput(e.target.value);
              if (saveError) setSaveError(null);
            }}
            placeholder="Describe your question in detail..."
            disabled={isSaving}
          />
          <div className="qa-actions">
            <button
              type="submit"
              disabled={isSaving || !qaTitle.trim() || !qaInput.trim()}
            >
              {isSaving ? "Posting..." : "Post Question"}
            </button>
            <button
              type="button"
              onClick={handleCancelForm}
              disabled={isSaving}
            >
              Cancel
            </button>
          </div>
        </form>
      )}

      <div className="qa-list">
        {qaComments.length === 0 && !isAddingQuestion && (
          <div className="empty">No questions yet — be the first.</div>
        )}

        {qaComments.map((question) => (
          <div key={question.id} className="qa-question-container">
            {currentUserId && currentUserId === question.askedBy && (
              <div className="qa-question-ownership">
                <span className="ownership-label">You asked this question</span>
                <div className="qa-question-actions">
                  <button
                    className="question-edit-btn question-action-btn"
                    title="Edit question"
                    onClick={() => handleStartEditQuestion(question)}
                    disabled={isSaving}
                  >
                    ✎
                  </button>
                  <button
                    className="question-delete-btn question-action-btn"
                    title="Delete question"
                    onClick={() => handleDeleteQuestion(question.id)}
                    disabled={isSaving}
                  >
                    ×
                  </button>
                </div>
              </div>
            )}

            {editingQuestionId === question.id ? (
              <div className="qa-form new-question-form">
                {saveError && (
                  <div
                    className="qa-error"
                    style={{
                      color: "#dc2626",
                      fontSize: "14px",
                      marginBottom: "8px",
                    }}
                  >
                    {saveError}
                  </div>
                )}
                <input
                  type="text"
                  className="qa-title-input"
                  value={editingQuestionTitle}
                  onChange={(e) => {
                    setEditingQuestionTitle(e.target.value);
                    if (saveError) setSaveError(null);
                  }}
                  placeholder="Question title"
                  disabled={isSaving}
                />
                <textarea
                  value={editingQuestionContent}
                  onChange={(e) => {
                    setEditingQuestionContent(e.target.value);
                    if (saveError) setSaveError(null);
                  }}
                  placeholder="Describe your question in detail..."
                  disabled={isSaving}
                />
                <div className="qa-actions">
                  <button
                    onClick={() => handleSaveEditQuestion(question.id)}
                    disabled={
                      isSaving ||
                      !editingQuestionTitle.trim() ||
                      !editingQuestionContent.trim()
                    }
                  >
                    {isSaving ? "Saving..." : "Save"}
                  </button>
                  <button
                    onClick={handleCancelEditQuestion}
                    disabled={isSaving}
                  >
                    Cancel
                  </button>
                </div>
              </div>
            ) : (
              <>
                <div
                  className="qa-question-header"
                  onClick={() => toggleQuestion(question.id)}
                >
                  <div className="qa-question-main">
                    <div className="qa-meta">
                      <strong>{question.askedBy}</strong> •{" "}
                      <small>{question.askedOn}</small>
                    </div>
                    <div className="qa-question-title">{question.title}</div>
                    <div className="qa-text">{question.content}</div>
                  </div>
                  <div className="qa-question-stats">
                    <span className="answer-count">
                      {question.answers?.length || 0}{" "}
                      {question.answers?.length === 1 ? "answer" : "answers"}
                    </span>
                    <span className="expand-icon">
                      {expandedQuestions[question.id] ? "▼" : "►"}
                    </span>
                  </div>
                </div>

                {expandedQuestions[question.id] && (
                  <div className="qa-answers-section">
                    {question.answers && question.answers.length > 0 ? (
                      <div className="answers-list">
                        {question.answers.map((answer) => (
                          <div key={answer.id} className="answer-item">
                            {currentUserId &&
                              currentUserId === answer.answeredBy && (
                                <div
                                  className="qa-question-ownership"
                                  style={{ marginBottom: 8 }}
                                >
                                  <span className="ownership-label">
                                    You answered
                                  </span>
                                  <div className="qa-question-actions">
                                    <button
                                      className="question-edit-btn question-action-btn"
                                      title="Edit answer"
                                      onClick={(e) => {
                                        e.stopPropagation();
                                        handleStartEditAnswer(answer);
                                      }}
                                      disabled={isSaving}
                                    >
                                      ✎
                                    </button>
                                    <button
                                      className="question-delete-btn question-action-btn"
                                      title="Delete answer"
                                      onClick={(e) => {
                                        e.stopPropagation();
                                        handleDeleteAnswer(answer.id);
                                      }}
                                      disabled={isSaving}
                                    >
                                      ×
                                    </button>
                                  </div>
                                </div>
                              )}

                            {editingAnswerId === answer.id ? (
                              <div className="qa-form new-question-form">
                                {saveError && (
                                  <div
                                    className="qa-error"
                                    style={{
                                      color: "#dc2626",
                                      fontSize: "14px",
                                      marginBottom: "8px",
                                    }}
                                  >
                                    {saveError}
                                  </div>
                                )}
                                <input
                                  type="text"
                                  className="qa-title-input"
                                  value={editingAnswerTitle}
                                  onChange={(e) => {
                                    setEditingAnswerTitle(e.target.value);
                                    if (saveError) setSaveError(null);
                                  }}
                                  placeholder="Answer title"
                                  disabled={isSaving}
                                />
                                <textarea
                                  value={editingAnswerContent}
                                  onChange={(e) => {
                                    setEditingAnswerContent(e.target.value);
                                    if (saveError) setSaveError(null);
                                  }}
                                  placeholder="Describe your answer..."
                                  disabled={isSaving}
                                />
                                <div className="qa-actions">
                                  <button
                                    onClick={() =>
                                      handleSaveEditAnswer(answer.id)
                                    }
                                    disabled={
                                      isSaving ||
                                      !editingAnswerTitle.trim() ||
                                      !editingAnswerContent.trim()
                                    }
                                  >
                                    {isSaving ? "Saving..." : "Save"}
                                  </button>
                                  <button
                                    onClick={handleCancelEditAnswer}
                                    disabled={isSaving}
                                  >
                                    Cancel
                                  </button>
                                </div>
                              </div>
                            ) : (
                              <>
                                <div className="answer-meta">
                                  <strong>{answer.answeredBy}</strong> •{" "}
                                  <small>{answer.answeredOn}</small>
                                </div>
                                <div className="answer-title">
                                  {answer.title}
                                </div>
                                <div className="answer-text">
                                  {answer.content}
                                </div>
                              </>
                            )}
                          </div>
                        ))}
                      </div>
                    ) : (
                      <div className="no-answers">No answers yet.</div>
                    )}

                    {answeringQuestionId === question.id ? (
                      <form
                        className="qa-form answer-form"
                        onSubmit={(e) => handleSubmitAnswer(e, question.id)}
                      >
                        {saveError && (
                          <div
                            className="qa-error"
                            style={{
                              color: "#dc2626",
                              fontSize: "14px",
                              marginBottom: "8px",
                            }}
                          >
                            {saveError}
                          </div>
                        )}
                        <input
                          type="text"
                          className="qa-title-input"
                          value={qaTitle}
                          onChange={(e) => {
                            setQaTitle(e.target.value);
                            if (saveError) setSaveError(null);
                          }}
                          placeholder="Answer title"
                          autoFocus
                          disabled={isSaving}
                        />
                        <textarea
                          value={qaInput}
                          onChange={(e) => {
                            setQaInput(e.target.value);
                            if (saveError) setSaveError(null);
                          }}
                          placeholder="Write your answer..."
                          disabled={isSaving}
                        />
                        <div className="qa-actions">
                          <button
                            type="submit"
                            disabled={
                              isSaving || !qaInput.trim() || !qaTitle.trim()
                            }
                          >
                            {isSaving ? "Posting..." : "Post Answer"}
                          </button>
                          <button
                            type="button"
                            onClick={handleCancelForm}
                            disabled={isSaving}
                          >
                            Cancel
                          </button>
                        </div>
                      </form>
                    ) : (
                      <button
                        className="answer-question-btn"
                        onClick={() => handleAnswerClick(question.id)}
                      >
                        Answer this question
                      </button>
                    )}
                  </div>
                )}
              </>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}
